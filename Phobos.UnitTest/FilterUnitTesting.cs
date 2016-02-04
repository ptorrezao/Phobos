using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phobos.Controllers;
using System.Web;
using Rhino.Mocks;
using Phobos.Library.Interfaces;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Specialized;
using Phobos.ActionFilter;

namespace Phobos.UnitTest
{
    [TestClass]
    public class FilterUnitTesting
    {
        private IAuthenticationService mockAuth;
        private MockRepository mockRepo;
        private HttpContextBase context;

        [TestInitialize]
        public void initialize()
        {
            mockRepo = new MockRepository();
            context = mockRepo.DynamicMock<HttpContextBase>();
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
            request.Expect(req => req.Url).Return(new Uri("http://localhos/Home/Index"));
            request.Expect(req => req.RequestContext).Return(requestContext);
            requestContext.Expect(x => x.RouteData).Return(new RouteData());
            request.Expect(req => req.Headers).Return(new NameValueCollection());

            this.mockAuth = mockRepo.DynamicMock<IAuthenticationService>();
        }
    }
}
