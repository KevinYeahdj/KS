using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinBrain.WorkFlowEngine.Business.Entity;
using ClinBrain.WorkFlowEngine.Business.Manager;

namespace ClinBrain.WorkFlowEngine.Service
{
    public class RoleService
    {
        public List<RoleEntity> GetRoleAll()
        {
            var roleManager = new RoleManager();
            return roleManager.GetAll();
        }
    }
}
