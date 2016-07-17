# Interfax.ClientLib

<table>
<tbody>
<tr>
<td><a href="#accounts">Accounts</a></td>
<td><a href="#base">Base</a></td>
</tr>
<tr>
<td><a href="#errorblock">ErrorBlock</a></td>
<td><a href="#inboundfax">InboundFax</a></td>
</tr>
<tr>
<td><a href="#inlineoutbounddocument">InlineOutboundDocument</a></td>
<td><a href="#linkedoutbounddocument">LinkedOutboundDocument</a></td>
</tr>
<tr>
<td><a href="#outbounddocument">OutboundDocument</a></td>
<td><a href="#outboundfaxfull">OutboundFaxFull</a></td>
</tr>
<tr>
<td><a href="#outboundfaxsummary">OutboundFaxSummary</a></td>
<td><a href="#uploadeddocument">UploadedDocument</a></td>
</tr>
<tr>
<td><a href="#documentdisposition">DocumentDisposition</a></td>
<td><a href="#documentstatus">DocumentStatus</a></td>
</tr>
<tr>
<td><a href="#pageorientation">PageOrientation</a></td>
<td><a href="#pagerendering">PageRendering</a></td>
</tr>
<tr>
<td><a href="#pageresolution">PageResolution</a></td>
<td><a href="#pagesize">PageSize</a></td>
</tr>
<tr>
<td><a href="#requeststatus">RequestStatus</a></td>
<td><a href="#sharing">Sharing</a></td>
</tr>
<tr>
<td><a href="#statusfamily">StatusFamily</a></td>
<td><a href="#inboundfax">InboundFax</a></td>
</tr>
<tr>
<td><a href="#outbounddocuments">OutboundDocuments</a></td>
<td><a href="#outboundfax">OutboundFax</a></td>
</tr>
<tr>
<td><a href="#mediatypefinder">MediaTypeFinder</a></td>
</tr>
</tbody>
</table>


## Accounts

Represents Accounts service

### Constructor(userId, Password, timeout, endPoint)

Constructor

| Name | Description |
| ---- | ----------- |
| userId | *System.String*<br>Interfax User Id |
| Password | *System.String*<br>Interfax password |
| timeout | *System.Nullable{System.TimeSpan}*<br>Request timeout (optional, default is 30 sec.) |
| endPoint | *System.String*<br>The service end point (optional). Default is live service |

### getBalance(balance)

Determine the remaining faxing credits in your account (in the account's currency).

| Name | Description |
| ---- | ----------- |
| balance | *System.Decimal@*<br>The found balance |

#### Returns

Request status


## Base

Base class for services classes

### Constructor(basePath, userId, password, timeout, endPoint)

Constructor

| Name | Description |
| ---- | ----------- |
| basePath | *System.String*<br>The base path for the specific service |
| userId | *System.String*<br>Interfax User Id |
| password | *System.String*<br>Interfax password |
| timeout | *System.Nullable{System.TimeSpan}*<br>Request timeout (optional, default is 30 sec.) |
| endPoint | *System.String*<br>The service end point (optional). Default is live service |

### client

The Http client to use

### DeleteUri(uri)

Sends a Delete Http request to a given Uri

| Name | Description |
| ---- | ----------- |
| uri | *System.String*<br>the Uri |

#### Returns

The request status

### ErrorBlock

Detailed error block of last operation

### ExtractErrorBlock(res)

Try to extract an error block from the response content and deserialize it

| Name | Description |
| ---- | ----------- |
| res | *System.Net.Http.HttpResponseMessage*<br>The response message object |

### GetBinary(uri, result)

Send a Get request and populate a binary response

| Name | Description |
| ---- | ----------- |
| uri | *System.String*<br>Request Uri |
| result | *System.Byte[]@*<br>Output: the resulting binary data |

#### Returns

The request status

### GetObject\`\`1(data, result)

Get an object of a given type from a binary content

#### Type Parameters

- T - The type of the resulting object

| Name | Description |
| ---- | ----------- |
| data | *System.Byte[]*<br>binary content |
| result | *\`\`0@*<br>Output: the resulting object |

#### Returns

The request status

### GetObject\`\`1(uri, result)

Send a Get request and populate a response to a .NET object of a given type

#### Type Parameters

- T - Type of result

| Name | Description |
| ---- | ----------- |
| uri | *System.String*<br>Request Uri |
| result | *\`\`0@*<br>Output: the resulting object |

#### Returns

The request status

### LastError

Error message of last operation

### ParseBinary(response, result)

Perse a response and get its content as binary data

| Name | Description |
| ---- | ----------- |
| response | *System.Net.Http.HttpResponseMessage*<br>The ResponseMessage object received from server |
| result | *System.Byte[]@*<br>The resulting byte array |

#### Returns

The request status

### Post(uri)

Send a POST request with no content and no returned object

| Name | Description |
| ---- | ----------- |
| uri | *System.String*<br>Request Uri |

#### Returns

The request status

### Post\`\`2(uri, input, result)

Send a POST request with content of a given type and populate a response on another type

#### Type Parameters

- Tin - Type of input
- Tout - Type of result

| Name | Description |
| ---- | ----------- |
| uri | *System.String*<br>Request Uri |
| input | *\`\`0*<br>Input object |
| result | *\`\`1@*<br>Output: the resulting object |

#### Returns

The request status

### PostAndGetLocation(uri, location, content)

Send a POST request with no content and no returned object

| Name | Description |
| ---- | ----------- |
| uri | *System.String*<br>Request Uri |
| location | *System.Uri@*<br>Output: The value in the Locationn Http header |
| content | *System.Net.Http.HttpContent*<br>Optional: the content to be posted |

#### Returns

The request status

### PostBinary(uri, data, offset, count)

Send a POST request with binary content and no returned object

| Name | Description |
| ---- | ----------- |
| uri | *System.String*<br>Request Uri |
| data | *System.Byte[]*<br>Binary data to be sent |
| offset | *System.Int32*<br>position in data to start transfer from |
| count | *System.Int32*<br>Number of bytes from offset to transfer |

#### Returns

The request status

### PostBinary(uri, data)

Send a POST request with binary content and no returned object

| Name | Description |
| ---- | ----------- |
| uri | *System.String*<br>Request Uri |
| data | *System.Byte[]*<br>Binary data to be sent |

#### Returns

The request status

### PostBinary(uri, content, range)

Send a POST request with binary content and no returned object

| Name | Description |
| ---- | ----------- |
| uri | *System.String*<br>Request Uri |
| content | *System.Net.Http.HttpContent*<br>Content to be posted |
| range | *System.Net.Http.Headers.RangeHeaderValue*<br>Optional: a range header to be set (default is null) |

#### Returns

The request status


## ErrorBlock

Represents detailed error reporting block

### Code

Code as returned from service

### Message

The base error message

### MoreInfo

Detailed information about the error


## InboundFax

Meta-data of an inbound fax

### CallerID

The caller ID of the sender

### MessageID

The ID of this transaction

### Pages

The number of pages received in this fax.

### PhoneNumber

The Phone number at which this fax was received

### ReceiveTime

The time and date that the fax was received (in GMT)

### RecordingDuration

The time (in seconds) that it took to receive the fax

### RemoteCSID

The CSID of the sender

### Status

Status of the fax. See the list of InterFAX Error Codes


## InlineOutboundDocument

Represents a single document with inline content to be faxes

### Constructor(data, fileType, charSet)

Constructor: Initialize with a byte array

| Name | Description |
| ---- | ----------- |
| data | *System.Byte[]*<br>The byte-array containing the document to be faxed |
| fileType | *System.String*<br>The type of the document to be faxed (e.g 'pdf') |
| charSet | *System.String*<br>In case of a text (e.g Html), this should be specified |

### Constructor(dataStream, fileType, closeStream, charSet)

Constructor: Initialize with a Stream

| Name | Description |
| ---- | ----------- |
| dataStream | *System.IO.Stream*<br>The IO stream containing the document to be faxed |
| fileType | *System.String*<br>The type of the document to be faxed (e.g 'pdf') |
| closeStream | *System.Boolean*<br>Optional (default=true), tells the method whether to close the stream after using its data |
| charSet | *System.String*<br>In case of a text (e.g Html), this should be specified |

### Constructor(path, charSet)

Constructor: Initialize with a file

| Name | Description |
| ---- | ----------- |
| path | *System.String*<br>The fully qualified path to the file containing the document to be faxed |
| charSet | *System.String*<br>In case of a text (e.g Html), this should be specified |

### CharSet

The character set encoding (applies in case of textual content)

### Data

The raw data

### FileType

The type of the document to be faxed. The File type must be in the list of supported file types as specifies in https://www.interfax.net/en/help/supported_file_types.


## LinkedOutboundDocument

Represents an already-uploaded document to be faxed

### UploadedDocument

The uri of the uploaded document as returned by calling Upload() method


## OutboundDocument

Represents a single document to be faxes


## OutboundFaxFull

Represents an outbound fax

### AttemptsToPerform

Maximum number of transmission attempts requested in case of fax transmission failure.

### Contact

Contact name provided during submission

### DestinationFax

The resolved fax number to which the fax was sent.

### PageHeader

The fax header text inserted at the top of the page.

### PageOrientation

portrait or landscape

### PageSize

Page size: a4, letter, legal, or b4

### PagesSubmitted

Total number of pages submitted.

### Rendering

greyscale or bw

### ReplyEmail

E-mail address(es) for confirmation message.

### Resolution

standard or fine

### SenderCSID

Sender's fax ID.

### Subject

A name or other optional identifier.

### SubmitTime

Time when the transaction was originally submitted. Always returned in GMT.


## OutboundFaxSummary

Represents an outbound fax summary information

### CompletionTime

End time of last of all transmission attempts. Always returned in GMT.

### CostPerUnit

Monetary units, in account currency. Multiply this by 'Units' to get tde actual cost of the fax.

### Duration

Transmission time in seconds.

### Id

A unique identifier for the fax.

### PagesSent

Number of successfully sent pages.

### Priority

For internal use.

### RemoteCSID

Receiving party fax ID (up to 20 characters).

### Status

Fax status. Generally, 0= OK; less than 0 = in process; greater than 0 = Error (See Interfax Status Codes)

### Units

Decimal number of units to be billed (pages or tenths of minutes)

### Uri

A unique resource locator for the fax.

### UserID

The submitting user.


## UploadedDocument

represents a single uploaded document meta data

### creationTime

Time (UTC) when the document was created

### disposition

Document disposition definition

### documentStatus

Current status of the document

### fileName

The filename provided when the document was uploaded, e.g. newsletter.pdf

### fileSize

The planned size in bytes of the document.

### lastUsageTime

Time (UTC) when the document was last used

### sharing

Document sharing definition

### uploaded

The number of bytes actually uploaded.

### uri

Fully-qualified resource URI of the document.

### userId

Username under which document was created.


## DocumentDisposition

Upload documents disposition

### MultiUse

deleted 60 minutes after the last usage

### Permanent

remains available until specifically removed by user

### SingleUse

can be used once


## DocumentStatus

Document upload status

### Created

Upload session was started, but no data has been uploaded

### Deleting

Document is being deleted

### PartiallyUploaded

Upload session was started, and some (not complete) data has been uploaded

### Ready

Document is ready to be used

### Uploading

Data upload is in progress


## PageOrientation

Represents supported page sizes

### Landscape

Landscape

### Portrait

Portrait


## PageRendering

Represents supported page sizes

### Fine

Grey scale; Optimized for image-intensive document

### Standard

Black and white; Optimized for text-intensive document


## PageResolution

Represents supported page sizes

### High

High resolution (approx. 200 x 200); Better quality with higher size and transmission time

### Standard

High resolution (approx. 100 x 200); Optimized size and transmission time


## PageSize

Represents supported page sizes

### A4

A4 page size (common outside North America)

### B4

B4 page size (used mainly in Japan)

### Legal

Legal page size (common in North America)

### Letter

Letter page size (common in North America)


## RequestStatus

Status for requests

### Accepted

Request accepted, but resource is not completed

### AuthenticationError

Authentication error

### BadParameters

The request has some bad input.

### Created

Resource created

### NotFound

Resource not found

### OK

Success

### OverLimits

Request was over limit(s)

### SystemError

System error

### TimedOut

Request time out


## Sharing

Sharing options

### Private

Only owner user can access the resource

### Shared

All users in the account can access the resource


## StatusFamily

Defines groups of statuses

### All

any status

### Completed

Completed faxes, whether successful or failed

### Failed

failed faxes

### Inprocess

faxes in process (not completed)

### Specific

A specific status

### Success

successfully-completed faxes


## InboundFax

Client for the Inbound fax service

### Constructor(userId, password, timeout, endPoint)

Constructor

| Name | Description |
| ---- | ----------- |
| userId | *System.String*<br>Interfax User Id |
| password | *System.String*<br>Interfax password |
| timeout | *System.Nullable{System.TimeSpan}*<br>Request timeout (optional, default is 30 sec.) |
| endPoint | *System.String*<br>The service end point (optional). Default is live service |

### GetImage(id, image)

Retrieve Image data for a specific fax, Format is determined by the user's setting

| Name | Description |
| ---- | ----------- |
| id | *System.Byte[]@*<br>Transaction Id |
| image | *System.Int32*<br>Output: The image data for the fax |

#### Returns

Request status

### GetList(list, lastId, unreadOnly, limit, allUsers)

Retrieves a user's list of inbound faxes. (Sort order is always by descending ID).

| Name | Description |
| ---- | ----------- |
| list | *System.Collections.Generic.IEnumerable{Interfax.ClientLib.Entities.InboundFax}@*<br>output: The list of meta data for each fax |
| lastId | *System.Boolean*<br>Optional: Return results from this ID backwards (not including this ID). Used for pagination. |
 //TODO: check sort order 
| Name | Description |
| ---- | ----------- |
| unreadOnly | *System.Int32*<br>Optional (default is false). Return only unread faxes? |
| limit | *System.Boolean*<br>Optional (default is 25). How many transactions to return. |
| allUsers | *System.Int32*<br>Optional (default is false). For a "primary" user, determines whether to return data for the current user only or for all account users. |

#### Returns

Request status

### GetMeta(id, meta)

Retrieve meta data for a specific fax

| Name | Description |
| ---- | ----------- |
| id | *Interfax.ClientLib.Entities.InboundFax@*<br>Transaction Id |
| meta | *System.Int32*<br>Output: The meta data for the fax |

#### Returns

Request status

### MarkRead(id, unread)

Mark an image as read (or unread)

| Name | Description |
| ---- | ----------- |
| id | *System.Int32*<br>Transaction Id |
| unread | *System.Boolean*<br>Optional: set false to mark as Unread |

#### Returns




## OutboundDocuments

Represents the document upload facility for outbound faxing

### Constructor(userId, Password, timeout, endPoint)

Constructor

| Name | Description |
| ---- | ----------- |
| userId | *System.String*<br>Interfax User Id |
| Password | *System.String*<br>Interfax password |
| timeout | *System.Nullable{System.TimeSpan}*<br>Request timeout (optional, default is 30 sec.) |
| endPoint | *System.String*<br>The service end point (optional). Default is live service |

### Delete(documentId)

Delete an uploaded document (also cancels an uploaded document session)

| Name | Description |
| ---- | ----------- |
| documentId | *System.String*<br>Id obtained during call to D |

#### Returns

The request status

### GetList(limit, offset, list)

Get a list of previous document uploads which are currently available.

| Name | Description |
| ---- | ----------- |
| limit | *System.Collections.Generic.IEnumerable{Interfax.ClientLib.Entities.UploadedDocument}@*<br>How many document references to return. |
| offset | *System.Int32*<br>Skip this many document references in the list. |
| list | *System.Int32*<br>Output: the list of documents |

#### Returns

The request status

### GetStatus(documentId, document)

Retrieve the meta data about a document

| Name | Description |
| ---- | ----------- |
| documentId | *Interfax.ClientLib.Entities.UploadedDocument@*<br> |
| document | *System.String*<br> |

#### Returns

The request status

### Upload(location, data, name, chunckSize, disposition, sharing)

Uploads an entire document in chuncks

| Name | Description |
| ---- | ----------- |
| location | *System.Uri@*<br>Output: the Uri for the created document |
| data | *System.Byte[]*<br>Document data |
| name | *System.String*<br>The document file name, which can subsequently be queried with the GetList() method. The filename must end with an extension defining the file type, e.g. dailyrates.docx or newsletter.pdf, and the file type must be in the list of supported file types as specifies in https://www.interfax.net/en/help/supported_file_types. |
| chunckSize | *System.Int32*<br>size in bytes for data to uload in each HTTP request |
| disposition | *Interfax.ClientLib.Enums.DocumentDisposition*<br>This sets the retention policy of the uploaded document, that is, how long it can be used by the POST |
| sharing | *Interfax.ClientLib.Enums.Sharing*<br>private or shared |

#### Returns

The request status


## OutboundFax

Proxy class for fax outbound service

### Constructor(userId, Password, timeout, endPoint)

Constructor

| Name | Description |
| ---- | ----------- |
| userId | *System.String*<br>Interfax User Id |
| Password | *System.String*<br>Interfax password |
| timeout | *System.Nullable{System.TimeSpan}*<br>Request timeout (optional, default is 30 sec.) |
| endPoint | *System.String*<br>The service end point (optional). Default is live service |

### CancelFax(id)

Cancel a fax in progress.

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>ID of the fax to be cancelled. |

#### Returns

The request status

### GetCompleted(list, Ids)

Get details for a subset of completed faxes from a submitted list. (Submitted id's which have not completed are ignored).

| Name | Description |
| ---- | ----------- |
| list | *System.Collections.Generic.IEnumerable{Interfax.ClientLib.Entities.OutboundFaxSummary}@*<br>Output: the returned list |
| Ids | *System.Collections.Generic.IEnumerable{System.String}*<br>List of transactions to query |

#### Returns

The request status

### GetImage(data, id)

Retrieve the fax image (TIFF file) of a submitted fax.

| Name | Description |
| ---- | ----------- |
| data | *System.Byte[]@*<br>If successful, the response returns a TIFF file (image/tiff) of the outgoing fax image. |
| id | *System.String*<br>The transaction ID of the fax for which to retrieve data. |

#### Returns

The request status

### GetList(list, lastId, limit, descendingOrder, userId)

Get a list of recent outbound faxes (which does not include batch faxes).

| Name | Description |
| ---- | ----------- |
| list | *System.Collections.Generic.IEnumerable{Interfax.ClientLib.Entities.OutboundFaxFull}@*<br>Output: the returned list |
| lastId | *System.String*<br>(optional) Return results from this ID onwards (not including this ID). Used for pagination. If not provided, MaxValue is set when sortOrder is descending; zero when sortOrder is ascending. |
| limit | *System.Int32*<br>(Optional) How many transactions to return. |
| descendingOrder | *System.Boolean*<br>Set to false for ascending order |
| userId | *System.String*<br>(Optional, Default is Current user provided in credentials). Enables a "primary" user to query for other account users' faxes. |

#### Returns

The request status

### GetStatus(meta, id)

Retrieves information regarding a previously-submitted fax, including its current status.

| Name | Description |
| ---- | ----------- |
| meta | *Interfax.ClientLib.Entities.OutboundFaxFull@*<br>The |
| id | *System.String*<br>The transaction ID of the fax for which to retrieve data. |

#### Returns

The request status

### HideFax(id)

Hide a fax from listing in queries (there is no way to unhide a fax).

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br> |

#### Returns



### MakeContent(document)

Create an Http content out of an abstract OutboundDocument

| Name | Description |
| ---- | ----------- |
| document | *Interfax.ClientLib.Entities.OutboundDocument*<br>The OutboundDocument to fax |

#### Returns

Http Content

### ResendFax(location, id, faxNumber)

The resent fax is allocated a new transaction ID.

| Name | Description |
| ---- | ----------- |
| location | *System.Uri@*<br>Output: the URI of the newly-created fax resource |
| id | *System.String*<br>ID of the fax to be cancelled. |
| faxNumber | *System.String*<br>A single fax number, e.g: +1-212-3456789 |

#### Returns

The request status

### Search(list, ids, reference, dateFrom, dateTo, statusFamily, status, userId, faxNumber, descendingOrder, offset, limit)

Search outbound faxes

| Name | Description |
| ---- | ----------- |
| list | *System.Collections.Generic.IEnumerable{Interfax.ClientLib.Entities.OutboundFaxFull}@*<br>Output: the returned list |
| ids | *System.Collections.Generic.IEnumerable{System.String}*<br>(Optional, default is no restriction) List of fax IDs |
| reference | *System.String*<br>(Optional, default is no restriction) The 'reference' parameter entered at submit time |
| dateFrom | *System.Nullable{System.DateTime}*<br>(Optional, default is no restriction) Lower bound of date range from which to return faxes (GMT) |
| dateTo | *System.Nullable{System.DateTime}*<br>(Optional, default is no restriction) Upper bound of date range frrom which to return faxes |
| statusFamily | *System.Nullable{Interfax.ClientLib.Enums.StatusFamily}*<br>(Optional, default is 'All') The status family of faxes to return |
| status | *System.Int32*<br>Must be used in case the statusFamily is set to 'Specific' |
| userId | *System.String*<br>(Optional, default is no restriction) Limit returned faxes to these user ID's. This parameter has effect only when the querying username is a "primary" user. |
| faxNumber | *System.String*<br>(Optional, default is no restriction) Limit returned faxes to this destination fax number. |
| descendingOrder | *System.Boolean*<br>Set to false for ascending order |
| offset | *System.Int32*<br>(Optional) How many transactions to return. |
| limit | *System.Int32*<br>(Optional) Skip this many records |

#### Returns

The request status

### Submit(location, faxNumber, data, fileType, charSet)

Submit a fax containing one document from byte array

| Name | Description |
| ---- | ----------- |
| location | *System.Uri@*<br>Output: the URI of the newly-created fax resource |
| faxNumber | *System.String*<br>A single fax number, e.g: +1-212-3456789 |
| data | *System.Byte[]*<br>The byte-array containing the document to be faxed |
| fileType | *System.String*<br>The type of the document to be faxed (e.g 'pdf') |
| charSet | *System.String*<br>In case of a text (e.g Html), this should be specified |

#### Returns

The request status; If successful, a 201 Created status is returned

### Submit(faxNumber, location, dataStream, fileType, closeStream, charSet)

Submit a fax containing one document from stream

| Name | Description |
| ---- | ----------- |
| faxNumber | *System.Uri@*<br>A single fax number, e.g: +1-212-3456789 |
| location | *System.String*<br>Output: the URI of the newly-created fax resource |
| dataStream | *System.IO.Stream*<br>The IO stream containing the document to be faxed |
| fileType | *System.String*<br>The type of the document to be faxed (e.g 'pdf') |
| closeStream | *System.Boolean*<br>Optional (default=true), tells the method whether to close the stream after using its data |
| charSet | *System.String*<br>In case of a text (e.g Html), this should be specified |

#### Returns

The request status; If successful, a 201 Created status is returned

### Submit(faxNumber, location, path, charSet)

Submit a fax containing one document from file

| Name | Description |
| ---- | ----------- |
| faxNumber | *System.Uri@*<br>A single fax number, e.g: +1-212-3456789 |
| location | *System.String*<br>Output: the URI of the newly-created fax resource |
| path | *System.String*<br>The fully qualified path to the file containing the document to be faxed The filename must end with an extension defining the file type, e.g. dailyrates.docx or newsletter.pdf, and the file type must be in the list of supported file types as specifies in https://www.interfax.net/en/help/supported_file_types. |
| charSet | *System.String*<br>In case of a text (e.g Html), this should be specified |

#### Returns

The request status; If successful, a 201 Created status is returned

### SubmitExtended(faxNumber, location, documents, contact, postponeTime, retriesToPerform, csid, pageHeader, reference, replyAddress, pageSize, fitToPage, pageOrientation, resolution, rendering)

Submit a fax with multiple documents and extended options

| Name | Description |
| ---- | ----------- |
| faxNumber | *System.Uri@*<br>A single fax number, e.g: +1-212-3456789 |
| location | *System.String*<br>Output: the URI of the newly-created fax resource |
| documents | *System.Collections.Generic.IEnumerable{Interfax.ClientLib.Entities.OutboundDocument}*<br>list of documents to be faxed, in that order of pages |
| contact | *System.String*<br>A name or other reference. The entered string will appear: (1) for reference in the outbound queue; (2) in the outbound fax header, if headers are configured; and (3) in subsequent queries of the fax object. |
| postponeTime | *System.Nullable{System.DateTime}*<br>Time to schedule the transmission. |
| retriesToPerform | *System.Nullable{System.Int32}*<br>Number of transmission attempts to perform, in case of fax transmission failure. |
| csid | *System.String*<br>Sender CSID (up to 20 ascii characters) |
| pageHeader | *System.String*<br>The fax header text to insert at the top of the page |
| reference | *System.String*<br>Provide your internal ID to a document. This parameter can be obtained by status query, but is not included in the transmitted fax message. |
| replyAddress | *System.String*<br>E-mail address to which feedback messages will be sent. |
| pageSize | *System.Nullable{Interfax.ClientLib.Enums.PageSize}*<br>page size |
| fitToPage | *System.Nullable{System.Boolean}*<br>True to fit an image to the designated page size |
| pageOrientation | *System.Nullable{Interfax.ClientLib.Enums.PageOrientation}*<br>portrait or landscape |
| resolution | *System.Nullable{Interfax.ClientLib.Enums.PageResolution}*<br>Documents rendered as fine may be more readable but take longer to transmit (and may therefore be more costly). |
| rendering | *System.Nullable{Interfax.ClientLib.Enums.PageRendering}*<br>Determines the rendering mode. bw is recommended for textual, black and white documents, while greyscale is better for greyscale text and for images. |

#### Returns



### SubmitTextDocument(location, faxNumber, data, fileType)

Submit a fax containing one document from byte array

| Name | Description |
| ---- | ----------- |
| location | *System.Uri@*<br>Output: the URI of the newly-created fax resource |
| faxNumber | *System.String*<br>A single fax number, e.g: +1-212-3456789 |
| data | *System.String*<br>The byte-array containing the document to be faxed |
| fileType | *System.String*<br>The type of the textual document to be faxed (e.g. html) |

#### Returns

The request status; If successful, a 201 Created status is returned

### textualTypes

List of supported textual file types


## MediaTypeFinder

Helper - get media type by file type

### #cctor

Static constructor - load xml into a dictionary

### GetMediaType(fileType)

Get media type by file type

| Name | Description |
| ---- | ----------- |
| fileType | *System.String*<br>file type |

#### Returns

media type
