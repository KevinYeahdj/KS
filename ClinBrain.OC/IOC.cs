using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinBrain.OC
{

    //仅作为抓取组织架构内上级或经理等与当前人员相关结点处理人使用  能固定人的尽量不要在本架构内抓取 kevin
    public interface IOC
    {
        /// <summary>
        /// 根据用户登录名获取用户信息
        /// </summary>s
        /// <param name="userName">用户登录名</param>
        /// <returns>用户信息</returns>
        UserInfo GetUserInfoByLoginName(string loginName);

        /// <summary>
        /// 获取关键字人的信息
        /// </summary>
        /// <param name="keyWord">关键字</param>
        /// <returns>用户信息</returns>
        List<UserInfo> GetUserInfoByKeyWord(string keyWord);

        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <param name="companyCode">公司编号</param>
        /// <returns>公司信息</returns>
        CompanyInfo GetCompanyInfoByCode(string companyCode);

        /// <summary>
        /// 根据部门编号获取部门全路径和部门经理
        /// </summary>
        /// <param name="deptId">部门编号</param>
        /// <returns>部门全路径</returns>
        string[] GetDepartmentPathAndManager(string deptId);

        /// <summary>
        /// 获取当前人员上级
        /// </summary>
        /// <param name="loginName">当前人员</param>
        /// <returns>人员信息</returns>
        List<UserInfo> GetSupervisor(string loginName);

        /// <summary>
        /// 获取当前人员的经理
        /// </summary>
        /// <param name="loginName">当前人员</param>
        /// <returns>人员信息</returns>
        List<UserInfo> GetManager(string loginName);


        /// <summary>
        /// 根据邮箱获取当前人信息
        /// </summary>
        /// <param name="emails">邮箱</param>
        /// <returns>员工编号</returns>
        string[] GetEmployeeCodeByEmails(string[] emails);

        /// <summary>
        /// 获取当前人员的二级经理
        /// </summary>
        /// <param name="employeeCode">当前员工编号</param>
        /// <returns>人员信息</returns>
        UserInfo GetSubManager(string employeeCode);

        /// <summary>
        /// 根据流程配置，获取流程任务接收人的OC架构人员信息
        /// </summary>
        /// <param name="OCType">1表示流程发起人，2表示发起人上级，3表示发起人经理，11表示当前人，12表示当前人上级，13表示当前人经理</param>
        /// <returns></returns>
        List<UserInfo> GetOCTypeParticipants(int OCType, string flowInitiator, string currentUser);
        

        
    }
}
