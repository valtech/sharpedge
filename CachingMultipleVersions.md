# Caching Multiple Versions of the Same Page #

It's entirely possible to vary the cache content of a page by arbitrary data such as cookies, query strings, etc.

You simply need to create a custom identifier:

```
namespace CustomIdentifiers : IEdgeIdentifier
{
   public class IdIdentifier
   {
      public string GetKey(HttpContext context)
      {
         HttpRequest request = context.Request;

         StringBuilder key = new StringBuilder();

         key.Append(request.QueryString["id"]);
   
         return key.ToString().ToLowerInvariant();
      }
   }
}
```

This can be added via the config:

```
<configuration>
   <sharpEdge>
      <identifiers>
         <add type="CustomIdentifiers.IdIdentifier, CustomIdentifiers"/>
      </identifiers>
   </sharpEdge>
</configuration>
```

If multiple identifiers are included in a rule, they are concatenated together. It's always a good idea to keep the default OriginUrlEdgeIdentifier in there.