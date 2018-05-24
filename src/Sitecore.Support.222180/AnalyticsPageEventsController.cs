﻿namespace Sitecore.Cintel.Endpoint
{
  using Sitecore;
  using Sitecore.Cintel.Configuration;
  using Sitecore.Cintel.Endpoint.Plumbing;
  using Sitecore.Cintel.Utility;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.DependencyInjection;
  using Sitecore.Marketing.Core;
  using Sitecore.Marketing.Definitions;
  using Sitecore.Marketing.Definitions.PageEvents;
  using Sitecore.Resources.Media;
  using System;
  using System.IO;
  using System.Net;
  using System.Net.Http;
  using System.Net.Http.Headers;
  using System.Runtime.InteropServices;
  using System.Web.Http;

  [AuthorizedReportingUserFilter]
  public class AnalyticsPageEventsController : ApiController
  {
    public object GetImage(Guid pageEventId, int w = 0, int h = 0)
    {
      byte[] imageData;
      string mimeType;
      if (ServiceLocator.ServiceProvider.GetDefinitionManagerFactory().GetDefinitionManager<IPageEventDefinition>().Get(pageEventId, Context.Culture) == null)
      {
        IMarketingImage image1 = ImageHelper.GetImageFromCoreDatabase(Guid.Parse(CustomerIntelligenceConfig.DefaultImageIds.PageEvent), w, h);
        imageData = image1.ImageData;
        mimeType = image1.MimeType;
        HttpResponseMessage message1 = new HttpResponseMessage(HttpStatusCode.OK)
        {
          Content = new ByteArrayContent(imageData)
        };
        message1.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
        message1.StatusCode = HttpStatusCode.OK;
        return message1;
      }
      Item mediaItem = ((Sitecore.Data.Fields.ImageField)Context.Database.GetItem(new ID(pageEventId)).Fields["Event Image"]).MediaItem;
      if (mediaItem == null)
      {
        IMarketingImage image2 = ImageHelper.GetImageFromCoreDatabase(Guid.Parse(CustomerIntelligenceConfig.DefaultImageIds.PageEvent), w, h);
        imageData = image2.ImageData;
        mimeType = image2.MimeType;
      }
      else
      {
        MediaOptions options = new MediaOptions
        {
          Thumbnail = true,
          UseMediaCache = true
        };
        if ((w > 0) && (h > 0))
        {
          options.Width = w;
          options.Height = h;
        }
        MediaStream stream1 = MediaManager.GetMedia(mediaItem).GetStream(options);
        mimeType = stream1.MimeType;
        Stream stream = stream1.Stream;
        imageData = new byte[stream.Length + 1L];
        stream.Read(imageData, 0, System.Convert.ToInt32(stream.Length));
      }
      HttpResponseMessage message2 = new HttpResponseMessage(HttpStatusCode.OK)
      {
        Content = new ByteArrayContent(imageData)
      };
      message2.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
      message2.StatusCode = HttpStatusCode.OK;
      return message2;
    }

    public object GetPageEvent(Guid pageEventId)
    {
      throw new NotImplementedException();
    }
  }
}
