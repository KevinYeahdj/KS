using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinBrain.WorkFlowEngine.Business.Entity;


namespace ClinBrain.WorkFlowEngine.Service
{
    public interface IRoleService
    {
        List<RoleEntity> GetRoleAll();
    }
}
