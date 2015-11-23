using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phobos.Library.Interfaces;
using Rhino.Mocks;
using System.Web.Mvc;
using Phobos.Controllers;
using Phobos.Library.Models.ViewModels;
using Phobos.Library.Models;
using System.Collections.Generic;
using System.Linq;
using Phobos.Library.TestServices;
using System.Web;
using System.Security.Principal;
using System.Web.Routing;
using System.Collections.Specialized;

namespace Phobos.UnitTest
{
    [TestClass]
    public class AccountControllerUnitTesting
    {
        private IUserManagementService usrMngSvc;
        private string localUser = "testUser";
        private string localPwd = "password";
        private IAuthenticationService mockAuth;

        [TestInitialize]
        public void initialize()
        {
            var mockRepo = new MockRepository();
            var context = mockRepo.DynamicMock<HttpContextBase>();
            var request = mockRepo.DynamicMock<HttpRequestBase>();
            var response = mockRepo.DynamicMock<HttpResponseBase>();
            var session = mockRepo.DynamicMock<HttpSessionStateBase>();
            var server = mockRepo.DynamicMock<HttpServerUtilityBase>();
            var user = mockRepo.DynamicMock<IPrincipal>();
            var identity = mockRepo.DynamicMock<IIdentity>();
            var urlHelper = mockRepo.DynamicMock<UrlHelper>();

            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var requestContext = mockRepo.DynamicMock<RequestContext>();
            requestContext.Expect(x => x.HttpContext).Return(context);
            context.Expect(ctx => ctx.Request).Return(request);
            context.Expect(ctx => ctx.Response).Return(response);
            context.Expect(ctx => ctx.Session).Return(session);
            context.Expect(ctx => ctx.Server).Return(server);
            context.Expect(ctx => ctx.User).Return(user);
            user.Expect(ctx => ctx.Identity).Return(identity);
            identity.Expect(id => id.IsAuthenticated).Return(true);
            identity.Expect(id => id.Name).Return("test");
            request.Expect(req => req.Url).Return(new Uri("http://www.google.com"));
            request.Expect(req => req.RequestContext).Return(requestContext);
            requestContext.Expect(x => x.RouteData).Return(new RouteData());
            request.Expect(req => req.Headers).Return(new NameValueCollection());



            this.usrMngSvc = new UserManagementService();

            this.mockAuth = mockRepo.DynamicMock<IAuthenticationService>();
            mockAuth.Expect(x => x.Login(localUser, true));
        }

        [TestMethod]
        public void AccountController_Login_ExistingUser()
        {
            AccountController controller = new AccountController(this.usrMngSvc, this.mockAuth);
            ActionResult loginAction = controller.Login();

            Assert.IsTrue(loginAction is ViewResult);
            ViewResult loginScreen = loginAction as ViewResult;

            if (loginScreen != null)
            {
                Assert.IsTrue(loginScreen.Model is AccountViewModel);
                var model = loginScreen.Model as AccountViewModel;
                model.UserName = this.localUser;
                model.Password = this.localPwd;
                model.RememberMe = true;

                loginAction = controller.Login(model);

                Assert.IsFalse(loginAction is ViewResult, string.Join(" |", loginScreen.ViewData.ModelState.Values.Select(x => x.Errors.First().ErrorMessage)));
                Assert.IsTrue(loginAction is RedirectToRouteResult);
            }
        }

        [TestMethod]
        public void AccountController_Login_NonExistingUser()
        {
            AccountController controller = new AccountController(this.usrMngSvc, this.mockAuth);
            ActionResult loginAction = controller.Login();

            Assert.IsTrue(loginAction is ViewResult);
            ViewResult loginScreen = loginAction as ViewResult;

            if (loginScreen != null)
            {
                Assert.IsTrue(loginScreen.Model is AccountViewModel);
                var model = loginScreen.Model as AccountViewModel;
                model.UserName = "fakeUser";

                loginAction = controller.Login(model);

                Assert.IsTrue(loginAction is ViewResult);
                Assert.IsTrue(loginScreen.ViewData.ModelState.Values.Count > 0, string.Join(" |", loginScreen.ViewData.ModelState.Values.Select(x => x.Errors.First().ErrorMessage)));
            }
        }

        [TestMethod]
        public void AccountController_Login_UnexpectedError()
        {
            AccountController controller = new AccountController(this.usrMngSvc, this.mockAuth);
            ActionResult loginAction = controller.Login();

            Assert.IsTrue(loginAction is ViewResult);
            ViewResult loginScreen = loginAction as ViewResult;

            if (loginScreen != null)
            {
                Assert.IsTrue(loginScreen.Model is AccountViewModel);
                var model = loginScreen.Model as AccountViewModel;
                model.UserName = Guid.NewGuid().ToString();

                loginAction = controller.Login(model);

                Assert.IsTrue(loginAction is ViewResult);
                Assert.IsTrue(loginScreen.ViewData.ModelState.Values.Count > 0, string.Join(" |", loginScreen.ViewData.ModelState.Values.Select(x => x.Errors.First().ErrorMessage)));
            }
        }

        [TestMethod]
        public void AccountController_Logout()
        {
            AccountController controller = new AccountController(this.usrMngSvc, this.mockAuth);
            ActionResult loginAction = controller.Logout();

            Assert.IsTrue(loginAction is RedirectToRouteResult);
        }
    }
}
