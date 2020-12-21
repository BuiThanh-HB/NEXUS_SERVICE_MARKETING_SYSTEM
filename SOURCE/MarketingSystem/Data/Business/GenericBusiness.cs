using Data.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Business
{
    public class GenericBusiness
    {
        public NEXUS_SystemEntities context;
        public NEXUS_SystemEntities cnn;
        public GenericBusiness(NEXUS_SystemEntities context = null)
        {
            if (context == null)
            {
                this.context = new NEXUS_SystemEntities();
            }
            cnn = this.context;
            //this.context.Configuration.AutoDetectChangesEnabled = false;
            //this.context.Configuration.ValidateOnSaveEnabled = false;
            //this.context.Configuration.LazyLoadingEnabled = false;
        }
    }
}
