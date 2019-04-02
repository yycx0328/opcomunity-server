using Mvc;

namespace WebSite.Areas.v1
{
    public class v1AreaRegistration : BaseAreaRoute 
    {
        public override string AreaName 
        {
            get 
            {
                return "v1";
            }
        }
    }
}