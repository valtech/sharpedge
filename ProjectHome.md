# SharpEdge #

**The advanced replacement for ASP.NET's built in output caching.**

SharpEdge has been designed to cache entire pages and supports a plugin architecture to control:

  * cache key generation

  * what pages to include in the cache

  * what cache to use

This library is a general purpose software cache designed to cache pages similarly to the behaviour of load balancer / reverse proxy / middleware based caching that sits in front of the web server.

  * it allows URLs to be included using a custom IEdgeFilter filter that can be used to flag if a request URL should be cached.

  * it allows different storage formats and mechanism to be used to cache the results via a custom IEdgeStore.

  * it fixes a bug in ASP.NET output caching that make's Microsoft's ASP.NET output caching incompatible with Sitecore.

  * it allows cache keys to be determined to support adaptive rendering via the IEdgeIdentifier interface.