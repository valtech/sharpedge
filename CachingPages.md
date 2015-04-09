# How to Cache Pages #

If you use the this [configuration](Configuration.md), you just need to add a custom filter:

```
namespace CustomFilters
{
   public class EdgeFilter : IEdgeFilter
   {
      public bool Include(EdgeCacheRule rule, OriginUrl url)
      {
         return true;
      }
   }
}
```

Which can be added to the `<filters>` section like so:

```
<configuration>
   <sharpEdge>
      <filters>
         <add type="CustomFilters.EdgeFilter, CustomFilters"/>
      </filters>
   </sharpEdge>
</configuration>
```