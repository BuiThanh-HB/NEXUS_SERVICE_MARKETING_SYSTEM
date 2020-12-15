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
        public ThisDataEntities context;
        public ThisDataEntities cnn;
        public GenericBusiness(ThisDataEntities context = null)
        {
            if (context == null)
            {
                this.context = new ThisDataEntities();
            }
            cnn = this.context;
            //this.context.Configuration.AutoDetectChangesEnabled = false;
            //this.context.Configuration.ValidateOnSaveEnabled = false;
            //this.context.Configuration.LazyLoadingEnabled = false;
        }
    }
}
