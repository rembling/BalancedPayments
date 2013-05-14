Author: Russell Embling
Email: rembling@gmail.com
GitHub: https://github.com/rembling

Date: 5/14/2013

Overview:

This is a C# client library for interacting with the BalancedPayments API. Basically, I rewrote their existing java client library
found here (https://github.com/balanced/balanced-java). That being said, my version is pretty rudimentary; it differs slightly in design from the java client library
and leaves some functionality out, whatever I deemed not necessary for an MVP (minimal valueable product) launch. I included those files in the "java_client_files" folder. 
I'm sharing it in the hopes it helps as a starting point for anyone else, and I'd love help from the open source community in 
making it better. Note: I recently became aware of another C# project on GitHub (https://github.com/rentler/BalancedSharp) that 
looks to be very thorough. You should also reference that one. As for this one, I did include a console app which walks through the basic features:

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

Here are areas where I'd love help from the open source community to improve this client library: 

- Develop/Architect a better implementation of IoC (Inversion of Control): I gave a hack at it, but it's probably less than desireable. The JSon Deserialization seems to require a base constructor (with a no-parameter signature) on all classes, which I realized after writing my IoC, hence the signature with no parameters on classes (it's only there for the JSon conflict). 
- Solidify Error handling: I pretty much left this out
- Test project: didn't include one
- Add a Web Project with example credit card tokenization: BP's online documentation is really good, but including on in this solution would be pretty cool. I didn't include mine since it got pretty complex with all the other stuff I implemented. Perhaps if I have time I'll add a strip-down version.
- Develop the Query'ing and Paging functionality that the java 

Additional Resources:
- https://www.balancedpayments.com/docs/overview?language=bash
- https://www.balancedpayments.com/docs/api?language=bash
- https://www.balancedpayments.com/flow
- https://www.balancedpayments.com/help
