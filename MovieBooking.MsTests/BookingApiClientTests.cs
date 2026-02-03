using Microsoft.AspNetCore.Http;
using Moq;
using MovieBooking.Web.ApiContracts.Booking;
using MovieBooking.Web.Services;
using MovieBooking.Web.ViewModels.Booking;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace MovieBooking.MsTests
{
    [TestClass]
    public class BookingApiClientTests
    {
        private BookingMvcService CreateService(HttpResponseMessage response)
        {
            // ----- HttpClient -----
            var handler = new MockHttpMessageHandler(response);
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost")
            };

            // ----- HttpContext with token -----
            var claims = new[]
            {
            new Claim("access_token", "test-token")
        };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = user
            };

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock
                .Setup(x => x.HttpContext)
                .Returns(httpContext);

            return new BookingMvcService(
                httpClient,
                httpContextAccessorMock.Object
            );
        }
        [TestMethod]
        public async Task LockSeatsAsync_Success_ReturnsMappedResult()
        {
            var apiResponse = new LockSeatsResultResponse
            {
                Success = true,
                Message = "Seats locked",
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                LockedSeats = new List<LockedSeatResponse>
            {
                new LockedSeatResponse
                {
                    SeatId = Guid.NewGuid(),
                    SeatNumber = "A1",
                    Price = 250
                }
            }
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(apiResponse),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            var service = CreateService(httpResponse);

            var model = new LockSeatsViewModel
            {
                ShowTimeId = Guid.NewGuid(),
                SeatIds = new List<Guid> { Guid.NewGuid() }
            };

            var result = await service.LockSeatsAsync(model);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Seats locked", result.Message);
            Assert.AreEqual(1, result.LockedSeats.Count);
            Assert.AreEqual("A1", result.LockedSeats[0].SeatNumber);
        }

        // ================= FAILURE TEST =================
        [TestMethod]
        public async Task LockSeatsAsync_Failure_ThrowsException()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Seats already locked")
            };

            var service = CreateService(httpResponse);

            await Assert.ThrowsExceptionAsync<Exception>(
                () => service.LockSeatsAsync(new LockSeatsViewModel()));
        }
    }
}
