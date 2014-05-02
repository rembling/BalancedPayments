BalancedPayments
================

A c# library for integrating with BalancedPayments

﻿Author: Russell Embling
Email: rembling@gmail.com
GitHub: https://github.com/rembling

Date: 5/14/2013

Overview:

This is a C# client library for interacting with the BalancedPayments API. Basically, I rewrote their existing java client library
found here (https://github.com/balanced/balanced-java). That being said, my version is pretty rudimentary; it differs slightly in design from the java client library
and leaves some functionality out, whatever I deemed not necessary for an MVP (minimal valueable product) launch. I included those files in the "java_client_files" folder. 
I'm sharing it in the hopes it helps as a starting point for anyone else, and I'd love help from the open source community in 
making it better. Note: I recently became aware two other C# projects on GitHub:
 https://github.com/rentler/BalancedSharp - looks to be very thorough. 
 https://github.com/balanced/balanced-csharp - I believe this one is being endorsed by BalancedPayments

As for mine, I did include a console app which walks through the basic features of using the client library:
 
Basic Features:

- create a buyer account
- get a buyer account
- tokenize a credit card
- associate a credit card with a buyer account
- charge/debit a credit card
- refund a credit card
- delete a credit card
- create a merchant account
- get a merchant account
- tokenize a bank account
- associate a bank account with a merchant account
- pay/credit a bank account
- add bank verification
- confirm bank verification

Here are areas where I'd love help from the open source community to improve this client library: 

- Develop/Architect a better implementation of IoC (Inversion of Control): I gave a hack at it, but it's probably less than desireable. The JSon Deserialization seems to require a base constructor (with a no-parameter signature) on all classes, which I realized after writing my IoC, hence the signature with no parameters on classes (it's only there for the JSon conflict). 
- Solidify Error handling: I pretty much left this out
- Test project: didn't include one; however, as previously noted, I did include a Console App which executes some of the basic features.
- Add a Web Project with example page showing the Javascript credit card tokenization: BP's online documentation is really good and I had no problems implementing this, but I didn't include my page. Perhaps if I have time I'll add a strip-down version.
- Develop the Query'ing and Paging functionality that the java client library has. This libary can be used to get targeted accounts, but will hit a limitation if you're trying to bring back large lists, due to the fact I left this functionality out. 

Additional Resources:
- https://www.balancedpayments.com/docs/overview?language=bash
- https://www.balancedpayments.com/docs/api?language=bash
- https://www.balancedpayments.com/flow
- https://www.balancedpayments.com/help
