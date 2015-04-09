# Configuration #

Although SharpEdge can be configured entirely via its API in code, it can also be configured via the web.config:

```
<configuration>
   <configSections>
      <section name="sharpEdge" type="SharpEdge.Configuration.EdgeCacheSection, SharpEdge" />
   </configSections>
   <sharpEdge>
      <rules>
         <add name="main" duration="30" debug="true">
            <stores>
               <add type="SharpEdge.OutputCacheEdgeStore, SharpEdge" />
               <add type="SharpEdge.ClientCacheEdgeStore, SharpEdge" />
            </stores>
            <identifiers>
               <add type="SharpEdge.OriginUrlEdgeIdentifier, SharpEdge" />
            </identifiers>
            <filters>
          
            </filters>
         </add>
      </rules>
   </sharpEdge>
   <system.webServer>
      <modules runAllManagedModulesForAllRequests="true">
         <add name="sharpEdgeModule" type="SharpEdge.EdgeCacheModule, SharpEdge"/>
      </modules>
   </system.webServer>
</configuration>
```

NOTE: for pages to be included in the cache, you would have to implement a custom filter. SharpEdge ignores the @Cache page directive which allows it to run side-by-side with normal ASP.NET output caching.