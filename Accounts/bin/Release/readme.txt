
请在web.config里配置如下：

 <appSettings>
    <!-- 连接字符串是否加密 -->
    <add key="ConStringEncrypt" value="false"/>
    <!-- 连接字符串 -->    
    <add key="ConnectionString" value="server=127.0.0.1;database=codematic;uid=sa;pwd="/>   
   <!-- 权限角色管理模块连接字符串 --> 
   <add key="ConnectionStringAccounts" value="server=127.0.0.1;database=codematic;uid=sa;pwd="/>
</appSettings>