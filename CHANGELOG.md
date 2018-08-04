<a name="1.0.0"></a>
<a name="1.0.1"></a>
# 1.0.1 (2016-09-16)

* Initial release with nuget package

<a name="1.0.2"></a>
# 1.0.2 (2016-12-11)

* ToString for Errors
* Added message to ApiException by calling base class
* Added methods to send FileStreams

<a name="1.0.3"></a>
# 1.0.3 (2017-01-28)

* Changed InboundFax.MessageId from string to int

<a name="1.0.4"></a>
# 1.0.4 (2017-02-09)

* Changed OutboundFax.PageHeader from int (!) to string
* Fixed a bug in Outbound.GetCompleted so that it gets completed faxes now.
* Signature change in GetCompleted from IEnumerable<int> to params int[] (not considered breaking as it wasn't working before)

<a name="1.0.5"></a>
# 1.0.5 (2017-04-18)

* Added support for image status DONT_EXIST in InfoundFax 

<name="2.0.2"></a>
# 2.0.2 (2018-08-4)

* Convert library to .NET Standard 2.0
* Convert tests to .NET Standard and dotnet test support
* Add support for .NET Core
* Add handling for valid 204 API responses (Thanks @ricky-shake-n-bake-bobby)
