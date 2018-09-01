# MockDbConnections

This repository contains an example of mockable abstract classes for .Net the IDbConnection interface and related interfaces. 

The mock example resides on the master branch.  This branch contains a parallel implementation that contains database integration tests.

Database access is challenging to unit test because databases exist separately from application code under test.  Therefore any unit test involving database access is really an integration test.

Read more at:

https://graciesdad.wordpress.com/2018/09/01/unit-testing-databases-with-nsubstitute/

