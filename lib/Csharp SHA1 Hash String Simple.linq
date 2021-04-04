<Query Kind="Statements">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

BitConverter.ToString(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes("X-Hashcash: 1:8:000101000000:Hello World::6AMAAA:rQEAIA"))).Dump();