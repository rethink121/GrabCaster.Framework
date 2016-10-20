# GrabCaster

Author: Nino Crudele (@ninocrudele)  
Blog: http://ninocrudele.me  
Info: http://GrabCaster.io  

GrabCaster is designed to allow end users, developers and IT users to integrate information and data into a variety of applications and systems, simply and easily.
GrabCaster eliminates all existing barriers by providing a new dynamic and easy way to integrate technologies and software.
GrabCaster includes the building blocks developers need to integrate successful, any kind of software and technology, send data across the network and internet, plug-and-play applications, vertical applications and operating systems, everything from track-and-trace to asset tracking, inventory control and system readiness and more, the only limit is our capacity for invention.
GrabCaster connects any kind of technology, custom programs, operating system stacks, it works with any existing line of business applications such as Enterprise Resource Planning (ERP) systems, Warehouse Management Systems (WMS), hardware devices and more specialized vertical software. This flexibility allows it to work seamlessly and in almost all cases automatically with minimal implementation required.
More information at http://grabcaster.io

# Prepare the development environment
Clone the repository.  
Open GrabCaster.sln in Visual Studio and build the solution.  
Execute the cmd script PrepareDevEnvironment.cmd under the Develoment Script solution folder.  
Now the development environment is ready to run in debug mode.  

# Packaging
To prepare a new GrabBaster msi package build the Setup project in the GrabCaster solution.

# How to Contribute

Contributions are very welcome, GrabCaster provides open-source access to it projects.
You can communicate your ideas trough the [contact form](http://grabcaster.com/grabcaster-contact/) or adding a [new issue](https://github.com/ninocrudele/GrabCaster.Framework/issues) in GitHub.

## Getting started

* Pick an [issue](https://github.com/ninocrudele/GrabCaster.Framework/issues) you're interested in doing or create a new one, something that you feel is missing.
* Send a "pull request".

## Create a pull request

* Fork GrabCaster/Public repo, refer to [Fork A Repository in the GitHub documentation](https://help.github.com/articles/fork-a-repo/) for more information.
* Clone your fork, refer to [Cloning A Repository in the GitHub documentation](https://help.github.com/articles/cloning-a-repository/) for more information.
* Make changes to your local copy, you can find a good example [here](https://git-scm.com/book/en/v2/Git-Basics-Recording-Changes-to-the-Repository) for more information.
* Push your local changes to your fork, refer to [Pushing To A Remote in the GitHub documentation](https://help.github.com/articles/pushing-to-a-remote/)
* Send me a pull request, refer to [Using Pull Requests @ GitHub docs](https://help.github.com/articles/using-pull-requests/) for more information.

Now your changes become visible to me and I can then review it, and we can discuss each line of code if necessary.

If you push additional changes to your fork during this process, the changes become immediately available in the pull request.

# GrabCaster License
When you clone the GrabCaster/main repository from GitHub, the author is licensing the software to you and/or your organization, under the Reciprocal Public License, version 1.5. Like any license, the RPL states the terms and conditions under which you are authorized to use and modify the software. 
The key points are:

* There is no cost to license the software or the source code, it is available to you to download.
* You can build it and deploy it as many times as you like.
* You can redistribute it, in executable or source-code form, to anyone you like, as long as you do so also under the RPL.
* You are free to modify the source code, either for your own purposes or to provide to others. However, the license requires that you contribute these modifications (in legal terms, derivative works) back to the community by creating a pull request in GitHub.
* If you are a private industry contributor, you are welcome to download and build the source code, use it in the projects you deploy to your clients, and use it in your software. Just note that if you create derivative works, you must contribute those back to the project, and if you distribute (whether for a fee or not) the software, you must do so under the RPL.

Note that it is entirely possible to build components that interoperate with the GrabCaster but are not themselves derivative works.
For instance, a software vendor may build a Trigger or Event to interface to its proprietary system, and expose that interface to other GrabCaster components such as new integration stack or the federated query intermediaries or for example creating any kind of UI or tool.

As long as this adapter, application, component or extension does not involve modification to existing GrabCaster internal framework, RPL-licensed GrabCaser source code, it would most likely not be considered a derivative work and can be licensed under any terms the vendor chooses.

It is considered GrabCaster Framework any file under the GrabCaster.Framework namespace.

The Reciprocal Public License 1.5 (RPL1.5) license is described here: 
http://www.opensource.org/licenses/rpl1.5.txt

For any question you can contact the author here:
http://grabcaster.com/grabcaster-contact/ or http://ninocrudele.me


# External Licenses

#### Microsoft Roslyn is used in GrabCaster.Framework.Engine component

Microsoft Roslyn is licensed under the Apache license as described here http://www.apache.org/licenses/
Microsoft Roslyn binaries are linked into the GrabCaster Framework distribution allowed under the license terms found here https://github.com/dotnet/roslyn/blob/master/License.txt.

#### StackExchange.Redis is used in GrabCaster.Framework.Dcp.Redis component

StackExchange.Redis is licensed under the MIT license as described here https://opensource.org/licenses/MIT
StackExchange.Redis binaries are linked into the GrabCaster Framework distribution allowed under the license terms found here https://github.com/StackExchange/StackExchange.Redis/blob/master/LICENSE.
