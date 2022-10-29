In order to enable Put/Delete on your web server (if it isn't working), add this to the web.config file that is in the root of your publish directory: 

<system.webServer>
	<modules runAllManagedModulesForAllRequests="false">
		<remove name="WebDAVModule" />
	</modules>
</system.webServer>