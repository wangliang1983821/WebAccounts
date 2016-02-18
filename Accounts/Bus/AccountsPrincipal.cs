using System;
using System.Collections;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
namespace LTP.Accounts.Bus
{
    /// <summary>
    /// �û�����İ�ȫ��������Ϣ
    /// </summary>
	public class AccountsPrincipal: System.Security.Principal.IPrincipal
    {
        Data.User dataUser = new Data.User();		

        #region ����
      //  protected System.Security.Principal.IIdentity identity;
		protected ArrayList permissionList;
		protected ArrayList permissionListid;
		protected ArrayList roleList;

        public int UserID { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// ��ǰ�û������н�ɫ
        /// </summary>
		public ArrayList Roles
		{
			get 
			{
				return roleList;
			}
		}
        /// <summary>
        /// ��ǰ�û�ӵ�е�Ȩ���б�
        /// </summary>
		public ArrayList Permissions
		{
			get 
			{
				return permissionList;
			}
		}
        /// <summary>
        /// ��ǰ�û�ӵ�е�Ȩ��ID�б�
        /// </summary>
		public ArrayList PermissionsID
		{
			get 
			{
				return permissionListid;
			}
		}
        // IPrincipal Interface Requirements:
        /// <summary>
        /// ��ǰ�û��ı�ʶ����
        /// </summary>
        [ScriptIgnore]
		public System.Security.Principal.IIdentity Identity
		{
            get { throw new NotImplementedException(); }
            //get 
            //{
            //    return identity;
            //}
            //set 
            //{
            //    identity = value;
            //}
        }
        #endregion

        public AccountsPrincipal()
        {
            UserID = 0;

            UserName = string.Empty;
        }

        /// <summary>
        /// �����û���Ź���
        /// </summary>
        public AccountsPrincipal(int userID)
		{
            UserID = userID;
            //identity = new SiteIdentity(userID);
			permissionList = dataUser.GetEffectivePermissionList(userID);
			permissionListid=dataUser.GetEffectivePermissionListID(userID);
			roleList = dataUser.GetUserRoles(userID);
		}
        /// <summary>
        /// �����û�������
        /// </summary>
		public AccountsPrincipal(string userName)
		{
            UserName = userName;
            UserID = new SiteIdentity(userName).UserID;

            permissionList = dataUser.GetEffectivePermissionList(UserID);
            permissionListid = dataUser.GetEffectivePermissionListID(UserID);
            roleList = dataUser.GetUserRoles(UserID);
		}

        public  void Init()
        {
            if (UserID != 0)
            {
                //identity = new SiteIdentity(UserID);
                permissionList = dataUser.GetEffectivePermissionList(UserID);
                permissionListid = dataUser.GetEffectivePermissionListID(UserID);
                roleList = dataUser.GetUserRoles(UserID);
            }
            else if (!string.IsNullOrEmpty(UserName))
            {
                UserID = new SiteIdentity(UserName).UserID;

                permissionList = dataUser.GetEffectivePermissionList(UserID);
                permissionListid = dataUser.GetEffectivePermissionListID(UserID);
                roleList = dataUser.GetUserRoles(UserID);
            }

        }

        /// <summary>
        /// ��ǰ�û��Ƿ�����ָ�����ƵĽ�ɫ
        /// </summary>
		public bool IsInRole(string role)
		{
            if (roleList == null)
            {
                Init();
            }
			return roleList.Contains( role );
		}

        public bool IsInUser(string user)
        {
            
            return dataUser.HasUser(user);
        }

        /// <summary>
        /// ��ǰ�û��Ƿ�ӵ��ָ�����Ƶ�Ȩ��
        /// </summary>
		public bool HasPermission( string permission )
		{
            if (permissionList == null)
            {
                Init();
            }
			return permissionList.Contains( permission );
		}
        /// <summary>
        /// ��ǰ�û��Ƿ�ӵ��ָ����Ȩ��
        /// </summary>
		public bool HasPermissionID(int permissionid )
		{
            if (permissionListid == null)
            {
                Init();
            }
			return permissionListid.Contains( permissionid );
		}
        /// <summary>
        /// ��֤��¼��Ϣ
        /// </summary>
		public static AccountsPrincipal ValidateLogin(string userName, string password)
		{
			int newID;
			byte[] cryptPassword = EncryptPassword( password );
            Data.User dataUser = new Data.User();
			if ((newID = dataUser.ValidateLogin(userName, cryptPassword)) > 0 )
				return new AccountsPrincipal( newID );
			else
				return null;			
		}
        /// <summary>
        /// �������
        /// </summary>
		public static byte[] EncryptPassword(string password)
		{
			UnicodeEncoding encoding = new UnicodeEncoding();
			byte[] hashBytes = encoding.GetBytes( password );			
			SHA1 sha1 = new SHA1CryptoServiceProvider();
			byte[] cryptPassword = sha1.ComputeHash ( hashBytes);
			return cryptPassword;
		}
		
	}
}
