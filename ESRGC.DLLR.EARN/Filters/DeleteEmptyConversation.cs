using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Controllers;

namespace ESRGC.DLLR.EARN.Filters
{
  public class DeleteEmptyConversation : ActionFilterAttribute
  {
    public override void OnActionExecuted(ActionExecutedContext filterContext) {
      base.OnActionExecuted(filterContext);

      var workUnit = (filterContext.Controller as BaseController).WorkUnit;
      try {
        //select all empty convo and delete them
        workUnit.ConversationRepository
          .Entities
          .Where(x => x.MessageBoards.Count() == 0)
          .ToList()
          .ForEach(c => {
            //delete its messages
            c.Messages.ToList().ForEach(m => workUnit.MessageRepository.DeleteEntity(m));
            //delete the convo
            workUnit.ConversationRepository.DeleteEntity(c);
          });
        workUnit.saveChanges();
      }
      catch {
                
      }
    }
  }
}