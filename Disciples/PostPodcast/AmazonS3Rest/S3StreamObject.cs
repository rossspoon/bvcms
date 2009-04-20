using System;
using System.IO;
using System.Collections;
using System.Text;

/// <summary>
/// Summary description for S3StreamObject
/// </summary>
/// 

namespace AmazonS3REST
{
    public class S3StreamObject
    {
		    private Stream stream;

            public Stream Stream {
                get {
                    return stream;
                }
            }

            private SortedList metadata;
            public SortedList Metadata {
                get {
                    return metadata;
                }
            }

            public S3StreamObject(Stream stream, SortedList metadata ) {
                this.stream = stream;
                this.metadata = metadata;
            }
        }
}