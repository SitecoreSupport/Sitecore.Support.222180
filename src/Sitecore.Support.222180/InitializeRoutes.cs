namespace Sitecore.Support.Cintel.Endpoint.Plumbing
{
  using Newtonsoft.Json.Serialization;
  using Sitecore.Cintel.Reporting;
  using Sitecore.Cintel.Search;
  using Sitecore.Pipelines;
  using System;
  using System.Web.Http;
  using System.Web.Http.ModelBinding;
  using System.Web.Http.ModelBinding.Binders;
  using System.Web.Routing;


  public class InitializeRoutes : Sitecore.Cintel.Endpoint.Plumbing.InitializeRoutes
  {

    public override void Process(PipelineArgs args)
    {
      this.RegisterRoutes(RouteTable.Routes, args);
    }

    protected override void RegisterRoutes(RouteCollection routes, PipelineArgs args)
    {
      base.RegisterRoutes(routes, args);
      if (routes.Remove(RouteTable.Routes["cintel_analytics_pageevents_image"]))
      {
        routes.MapHttpRoute("support_cintel_analytics_pageevents_image", "sitecore/api/ao/v1/analytics/pageevents/{pageEventId}/image",
        new { controller = "SupportAnalyticsPageEvents", action = "GetImage" }
        );
      }

    }
  }
}


