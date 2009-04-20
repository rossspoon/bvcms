// This software code is made available "AS IS" without warranties of any        
// kind.  You may copy, display, modify and redistribute the software            
// code either by itself or as incorporated into your code; provided that        
// you do not remove any proprietary notices.  Your use of this software         
// code is at your own risk and you waive any claim against Amazon               
// Digital Services, Inc. or its affiliates with respect to your use of          
// this software code. (c) 2006-2007 Amazon Web Services, Inc. or its             
// affiliates.          


using System;
using System.Collections;
using System.Net;
using System.Text;
using System.Web;
using System.IO;
using System.ComponentModel;

namespace AmazonS3REST
{
    /// An interface into the S3 system.  It is initially configured with
    /// authentication and connection parameters and exposes methods to access and
    /// manipulate S3 data.
    public class AWSAuthConnection
    {
        private string awsAccessKeyId;
        private string awsSecretAccessKey;
        private bool isSecure;
        private string server;
        private int port;

        #region AWSAuthConnection 
        // There are 4 ways to call this connection
        public AWSAuthConnection( string awsAccessKeyId, string awsSecretAccessKey ):
            this( awsAccessKeyId, awsSecretAccessKey, true )
        {
        }

        public AWSAuthConnection( string awsAccessKeyId, string awsSecretAccessKey, bool isSecure ):
            this( awsAccessKeyId, awsSecretAccessKey, isSecure, Utils.Host )
        {
        }

        public AWSAuthConnection( string awsAccessKeyId, string awsSecretAccessKey, bool isSecure,
                                  string server ) :
            this( awsAccessKeyId, awsSecretAccessKey, isSecure, server,
                  isSecure ? Utils.SecurePort : Utils.InsecurePort )
        {
        }

        public AWSAuthConnection( string awsAccessKeyId, string awsSecretAccessKey, bool isSecure,
                                  string server, int port )
        {
            this.awsAccessKeyId = awsAccessKeyId;
            this.awsSecretAccessKey = awsSecretAccessKey;
            this.isSecure = isSecure;
            this.server = server;
            this.port = port;
        }
        #endregion

        #region Create Bucket
        /// <summary>
        /// Creates a new bucket.
        /// </summary>
        /// <param name="bucket">The name of the bucket to create</param>
        /// <param name="headers">A Map of string to string representing the headers to pass (can be null)</param>
        public Response CreateBucket( string bucket, SortedList headers )
        {
            S3Object obj = new S3Object("", null);
            WebRequest request = MakeRequest("PUT", bucket, headers, obj);
            request.ContentLength = 0;
            request.GetRequestStream().Close();
            return new Response(request);
        }
        # endregion

        #region List Bucket
        /// <summary>
        /// Lists the contents of a bucket.
        /// </summary>
        /// <param name="bucket">The name of the bucket to list</param>
        /// <param name="prefix">All returned keys will start with this string (can be null)</param>
        /// <param name="marker">All returned keys will be lexographically greater than this string (can be null)</param>
        /// <param name="maxKeys">The maximum number of keys to return (can be 0)</param>
        /// <param name="headers">A Map of string to string representing HTTP headers to pass.</param>
        public ListBucketResponse ListBucket( string bucket, string prefix, string marker,
                                              int maxKeys, SortedList headers ) {
            return ListBucket( bucket, prefix, marker, maxKeys, null, headers );
        }

        /// <summary>
        /// Lists the contents of a bucket.
        /// </summary>
        /// <param name="bucket">The name of the bucket to list</param>
        /// <param name="prefix">All returned keys will start with this string (can be null)</param>
        /// <param name="marker">All returned keys will be lexographically greater than this string (can be null)</param>
        /// <param name="maxKeys">The maximum number of keys to return (can be 0)</param>
        /// <param name="headers">A Map of string to string representing HTTP headers to pass.</param>
        /// <param name="delimiter">Keys that contain a string between the prefix and the first
        /// occurrence of the delimiter will be rolled up into a single element.</param>
        public ListBucketResponse ListBucket( string bucket, string prefix, string marker,
                                              int maxKeys, string delimiter, SortedList headers ) {
            StringBuilder path = new StringBuilder( bucket );
            path.Append( "?" );
            if (prefix != null) path.Append("prefix=").Append(HttpUtility.UrlEncode(prefix)).Append("&");
            if ( marker != null ) path.Append( "marker=" ).Append(HttpUtility.UrlEncode(marker)).Append( "&" );
            if ( maxKeys != 0 ) path.Append( "max-keys=" ).Append(maxKeys).Append( "&" );
            if (delimiter != null) path.Append("delimiter=").Append(HttpUtility.UrlEncode(delimiter)).Append("&");
            // we've always added exactly one too many chars.
            path.Remove( path.Length - 1, 1 );

            return new ListBucketResponse(MakeRequest("GET", path.ToString(), headers));
        }
        #endregion

        #region Delete Bucket
        /// <summary>
        /// Deletes an empty Bucket.
        /// </summary>
        /// <param name="bucket">The name of the bucket to delete</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        /// <returns></returns>
        public Response DeleteBucket( string bucket, SortedList headers )
        {
            return new Response( MakeRequest( "DELETE", bucket, headers ) );
        }
        #endregion



        #region Encode Key for Signature
        // NOTE: The System.Net.Uri class does modifications to the URL.
        // For example, if you have two consecutive slashes, it will
        // convert these to a single slash.  This could lead to invalid
        // signatures as best and at worst keys with names you do not
        // care for.
        private static string encodeKeyForSignature(string key)
        {
            return HttpUtility.UrlEncode(key).Replace("%2f", "/");
        }
        #endregion

        #region Get Object
        /// <summary>
        /// Reads an object from S3
        /// </summary>
        /// <param name="bucket">The name of the bucket where the object lives</param>
        /// <param name="key">The name of the key to use</param>
        /// <param name="headers">A Map of string to string representing the HTTP headers to pass (can be null)</param>
        public GetResponse GetObject( string bucket, string key, SortedList headers )
        {
            return new GetResponse(MakeRequest("GET", bucket + "/" + encodeKeyForSignature(key), headers));
        }
        #endregion

        #region Put Object Using a Stream
        /// <summary>
        /// Puts a file to S3 using streamed IO
        /// </summary>
        public void PutObjectAsStream(string BucketName, string DestinationFileName, string LocalFilePath, SortedList MetaData)
        {
            FileInfo datafile = new FileInfo(LocalFilePath);
            Stream fileStream = File.OpenRead(LocalFilePath);
            S3StreamObject s3Object = new S3StreamObject(fileStream, null);
            PutStream(BucketName, DestinationFileName, s3Object, MetaData).getResponseMessage();
            fileStream.Close();
            fileStream.Dispose();
        }
        public void PutObjectAsStream(string BucketName, string DestinationFileName, string LocalFilePath, SortedList MetaData, BackgroundWorker worker, DoWorkEventArgs e)
        {
            FileInfo datafile = new FileInfo(LocalFilePath);
            Stream fileStream = File.OpenRead(LocalFilePath);
            S3StreamObject s3Object = new S3StreamObject(fileStream, null);
            PutStream(BucketName, DestinationFileName, s3Object, MetaData, worker, e);
            fileStream.Close();
            fileStream.Dispose();
        }

        #endregion

        #region PutStream
        /// <summary>
        /// Writes an object to S3 using streaming.
        /// </summary>
        /// <param name="bucket">The name of the bucket to which the object will be added.</param>
        /// <param name="key">The name of the key to use</param>
        /// <param name="obj">An S3Object containing the data to write.</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        public Response PutStream(string bucket, string key, S3StreamObject obj, SortedList headers)
        {

            Boolean isEmptyKey = (key == null) || (key.Length == 0)
                || (key.Trim().Length == 0);
            string pathSep = isEmptyKey ? "" : "/";

            if (key == null)
                key = "";

            WebRequest request = makeStreamRequest("PUT", bucket + pathSep
                + HttpUtility.UrlEncode(key), headers, obj);

            // cast WebRequest to a HttpWebRequest to allow for direct streaming of data
            HttpWebRequest hwr = (HttpWebRequest)request;
            hwr.AllowWriteStreamBuffering = false;
            hwr.SendChunked = false;
            hwr.ContentLength = obj.Stream.Length;

            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] buf = new byte[1024];
            BufferedStream bufferedInput = new BufferedStream(obj.Stream);
            int contentLength = 0;
            int bytesRead = 0;
            while ((bytesRead = bufferedInput.Read(buf, 0, 1024)) > 0)
            {
                contentLength += bytesRead;
                hwr.GetRequestStream().Write(buf, 0, bytesRead);
            }
            hwr.GetRequestStream().Close();

            return new Response(request);
        }
        public Response PutStream(string bucket, string key, S3StreamObject obj, SortedList headers, BackgroundWorker worker, DoWorkEventArgs e)
        {
            Boolean isEmptyKey = (key == null) || (key.Length == 0)
                || (key.Trim().Length == 0);
            string pathSep = isEmptyKey ? "" : "/";

            if (key == null)
                key = "";

            WebRequest request = makeStreamRequest("PUT", bucket + pathSep
                + HttpUtility.UrlEncode(key), headers, obj);

            // cast WebRequest to a HttpWebRequest to allow for direct streaming of data
            HttpWebRequest hwr = (HttpWebRequest)request;
            hwr.AllowWriteStreamBuffering = false;
            hwr.SendChunked = false;
            hwr.ContentLength = obj.Stream.Length;

            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] buf = new byte[1024];
            BufferedStream bufferedInput = new BufferedStream(obj.Stream);
            int contentLength = 0;
            int bytesRead = 0;
            while ((bytesRead = bufferedInput.Read(buf, 0, 1024)) > 0)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return null;
                }
                contentLength += bytesRead;
                hwr.GetRequestStream().Write(buf, 0, bytesRead);
                if (worker.WorkerReportsProgress)
                    worker.ReportProgress(Convert.ToInt32(Math.Round(Convert.ToDouble(contentLength) / Convert.ToDouble(hwr.ContentLength) * 100)));
            }
            hwr.GetRequestStream().Close();

            return new Response(request);
        }

        #endregion

        #region Delete Object
        /// <summary>
        /// Delete an object from S3.
        /// </summary>
        /// <param name="bucket">The name of the bucket where the object lives.</param>
        /// <param name="key">The name of the key to use.</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        /// <returns></returns>
        public Response DeleteObject( string bucket, string key, SortedList headers )
        {
            return new Response(MakeRequest("DELETE", bucket + "/" + encodeKeyForSignature(key), headers));
        }
        #endregion

        #region Get Bucket Logging
        /// <summary>
        /// Get the logging xml document for a given bucket
        /// </summary>
        /// <param name="bucket">The name of the bucket</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        public GetResponse GetBucketLogging(string bucket, SortedList headers)
        {
            return new GetResponse(MakeRequest("GET", bucket + "?logging", headers));
        }
        #endregion

        #region Put Bucket Logging
        /// <summary>
        /// Write a new logging xml document for a given bucket
        /// </summary>
        /// <param name="bucket">The name of the bucket to enable/disable logging on</param>
        /// <param name="loggingXMLDoc">The xml representation of the logging configuration as a String.</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        public Response PutBucketLogging(string bucket, string loggingXMLDoc, SortedList headers)
        {
            S3Object obj = new S3Object(loggingXMLDoc, null);

            WebRequest request = MakeRequest("PUT", bucket + "?logging", headers, obj);
            request.ContentLength = loggingXMLDoc.Length;

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(obj.Data);
            request.GetRequestStream().Write(bytes, 0, bytes.Length);
            request.GetRequestStream().Close();

            return new Response(request);
        }
        #endregion

        #region Get Bucket ACL
        /// <summary>
        /// Get the ACL for a given bucket.
        /// </summary>
        /// <param name="bucket">The the bucket to get the ACL from.</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        public GetResponse GetBucketACL(string bucket, SortedList headers)
        {
            return GetObjectACL(bucket, null, headers);
        }
        #endregion

        #region Get Object ACL
        /// <summary>
        /// Get the ACL for a given object
        /// </summary>
        /// <param name="bucket">The name of the bucket where the object lives</param>
        /// <param name="key">The name of the key to use.</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        public GetResponse GetObjectACL( string bucket, string key, SortedList headers )
        {
            if ( key == null ) key = "";
            return new GetResponse(MakeRequest("GET", bucket + "/" + encodeKeyForSignature(key) + "?acl", headers));
        }
        #endregion

        #region Set Bucket ACL
        /// <summary>
        /// Write a new ACL for a given bucket
        /// </summary>
        /// <param name="bucket">The name of the bucket to change the ACL.</param>
        /// <param name="aclXMLDoc">An XML representation of the ACL as a string.</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        public Response SetBucketACL(string bucket, string aclXMLDoc, SortedList headers)
        {
            return SetObjectACL(bucket, null, aclXMLDoc, headers);
        }
        #endregion

        #region Set Object ACL
        /// <summary>
        /// Write a new ACL for a given object
        /// </summary>
        /// <param name="bucket">The name of the bucket where the object lives or the
        /// name of the bucket to change the ACL if key is null.</param>
        /// <param name="key">The name of the key to use; can be null.</param>
        /// <param name="aclXMLDoc">An XML representation of the ACL as a string.</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        public Response SetObjectACL(string bucket, string key, string aclXMLDoc, SortedList headers)
        {
            S3Object obj = new S3Object( aclXMLDoc, null );
            if ( key == null ) key = "";

            WebRequest request = MakeRequest("PUT", bucket + "/" + encodeKeyForSignature(key) + "?acl", headers, obj);
            request.ContentLength = aclXMLDoc.Length;

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(obj.Data);
            request.GetRequestStream().Write(bytes, 0, bytes.Length);
            request.GetRequestStream().Close();

            return new Response(request);
        }
        #endregion

        public string AclPublicReadOnly()
        {
            const string AclPublicReadable =
                  "<Grant>"
                + "  <Grantee xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                + "    xsi:type=\"Group\"><URI> http://acs.amazonaws.com/groups/global/AllUsers</URI>"
                + "  </Grantee>"
                + "  <Permission>READ</Permission>"
                + "</Grant>";
            return AclPublicReadable;
        }


        #region List All My Buckets
        /// <summary>
        /// List all the buckets created by this account.
        /// </summary>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        public ListAllMyBucketsResponse ListAllMyBuckets( SortedList Headers )
        {
            return new ListAllMyBucketsResponse(MakeRequest("GET", "", Headers));
        }
        #endregion

        #region Make Request (Without an S3 Object)
        /// <summary>
        /// Make a new WebRequest without an S3Object.
        /// </summary>
        private WebRequest MakeRequest( string Method, string Resource, SortedList Headers )
        {
            return MakeRequest( Method, Resource, Headers, null );
        }
        #endregion

        #region Make Request (With the S3 Object)
        /// <summary>
        /// Make a new WebRequest
        /// </summary>
        /// <param name="method">The HTTP method to use (GET, PUT, DELETE)</param>
        /// <param name="resource">The resource name (bucket + "/" + key)</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        /// <param name="obj">S3Object that is to be written (can be null).</param>
        private WebRequest MakeRequest( string Method, string Resource, SortedList Headers, S3Object Obj )
        {
            string url = MakeURL( Resource );
            WebRequest req = WebRequest.Create( url );
            if (req is HttpWebRequest)
            {
                HttpWebRequest httpReq = req as HttpWebRequest;
                httpReq.AllowWriteStreamBuffering = false;
                
            }
            req.Method = Method;

            AddHeaders( req, Headers );
            if ( Obj != null ) AddMetadataHeaders( req, Obj.Metadata );
            AddAuthHeader( req, Resource );

            return req;
        }
        #endregion

        #region Make Stream Request
        /// <summary>
        /// Make a new WebRequest
        /// </summary>
        /// <param name="method">The HTTP method to use (GET, PUT, DELETE)</param>
        /// <param name="resource">The resource name (bucket + "/" + key)</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        /// <param name="obj">S3StreamObject that is to be written (can be null).</param>
        private WebRequest makeStreamRequest(string method, string resource, SortedList headers, S3StreamObject obj)
        {
            string url = MakeURL(resource);
            WebRequest req = WebRequest.Create(url);
            req.Timeout = 3600000; // 1 hr
            req.Method = method;

            AddHeaders(req, headers);
            if (obj != null) AddMetadataHeaders(req, obj.Metadata);
            AddAuthHeader(req, resource);

            return req;
        }

        #endregion

        #region Add HTTP Headers to Web Request
        /// <summary>
        /// Add the given headers to the WebRequest
        /// </summary>
        /// <param name="req">Web request to add the headers to.</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        private void AddHeaders( WebRequest req, SortedList headers )
        {
            AddHeaders( req, headers, "" );
        }
        #endregion

        #region Add MetaData to Headers
        /// <summary>
        /// Add the given metadata fields to the WebRequest.
        /// </summary>
        /// <param name="req">Web request to add the headers to.</param>
        /// <param name="metadata">A map of string to string representing the S3 metadata for this resource.</param>
        private void AddMetadataHeaders( WebRequest req, SortedList metadata )
        {
            AddHeaders( req, metadata, Utils.METADATA_PREFIX );
        }
        #endregion

        #region Add Headers to Request With a Prefix
        /// <summary>
        /// Add the given headers to the WebRequest with a prefix before the keys.
        /// </summary>
        /// <param name="req">WebRequest to add the headers to.</param>
        /// <param name="headers">Headers to add.</param>
        /// <param name="prefix">String to prepend to each before ebfore adding it to the WebRequest</param>
        private void AddHeaders( WebRequest req, SortedList headers, string prefix )
        {
            if ( headers != null )
            {
                foreach ( string key in headers.Keys )
                {
                    if (prefix.Length == 0 && key.Equals("Content-Type"))
                    {
                        req.ContentType = headers[key] as string;
                    }
                    else
                    {
                        req.Headers.Add(prefix + key, headers[key] as string);
                    }
                }
            }
        }
        #endregion

        #region Add Authorization Header to Request
        /// <summary>
        /// Add the appropriate Authorization header to the WebRequest
        /// </summary>
        /// <param name="request">Request to add the header to</param>
        /// <param name="resource">The resource name (bucketName + "/" + key)</param>
        private void AddAuthHeader( WebRequest request, string resource )
        {
            if ( request.Headers[Utils.ALTERNATIVE_DATE_HEADER] == null )
            {
                request.Headers.Add(Utils.ALTERNATIVE_DATE_HEADER, Utils.GetHttpDate());
            }

            string canonicalString = Utils.MakeCanonicalString( resource, request );
            string encodedCanonical = Utils.Encode( awsSecretAccessKey, canonicalString, false );
            request.Headers.Add( "Authorization", "AWS " + awsAccessKeyId + ":" + encodedCanonical );
        }
        #endregion

        #region Make URL
        /// <summary>
        /// Create a new URL object for the given resource.
        /// </summary>
        /// <param name="resource">The resource name (bucketName + "/" + key)</param>
        private string MakeURL( string Resource )
        {
            StringBuilder url = new StringBuilder();
            url.Append( isSecure ? "https" : "http" ).Append( "://" );
            url.Append( server ).Append( ":" ).Append( port ).Append( "/" );
            url.Append( Resource );
            return url.ToString();
        }
        #endregion
    }
}
