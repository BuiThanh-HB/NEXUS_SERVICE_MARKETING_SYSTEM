using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.APIWeb
{
    public class LoginOutputModel
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public int Role { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}
