using System;
using CookComputing.XmlRpc;

namespace CMSPresenter
{
    public struct BlogInfo
    {
        public string blogid;
        public string url;
        public string blogName;
    }


    // TODO: following attribute is a temporary workaround
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Enclosure
    {
        public int length;
        public string type;
        public string url;
    }

    // TODO: following attribute is a temporary workaround
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Source
    {
        public string name;
        public string url;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Post
    {
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required")]
        public DateTime dateCreated;
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required")]
        public string description;
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required")]
        public string title;

        public string[] categories;
        public Enclosure enclosure;
        public string link;
        public string postabstract;
        public string permalink;

        [XmlRpcMember(Description = "Not required when posting. Type may be either int or string")]
        public object postid;
        public Source source;
        public string userid;
    }

    public struct CategoryInfo
    {
        public string description;
        public string htmlUrl;
        public string rssUrl;
        public string title;
        public string categoryid;
    }

    public struct Category
    {
        public string categoryId;
        public string categoryName;
    }

    public struct MediaObject
    {
        public string name;
        public string type;
        public byte[] bits;
    }

    public struct MediaObjectUrl
    {
        public string url;
    }

    public interface IMetaWeblog
    {
        [XmlRpcMethod("metaWeblog.editPost", Description = "Updates and existing post to a designated blog "
          + "using the metaWeblog API. Returns true if completed.")]
        bool editPost(
         string postid,
         string username,
         string password,
         Post post,
         bool publish);

        [XmlRpcMethod("metaWeblog.getCategories",
          Description = "Retrieves a list of valid categories for a post " +
                        "using the metaWeblog API. Returns the metaWeblog categories " +
                        "struct collection.")]
        CategoryInfo[] getCategories(
          object blogid,
          string username,
          string password);

        [XmlRpcMethod("metaWeblog.getPost",
          Description = "Retrieves an existing post using the metaWeblog " +
                        "API. Returns the metaWeblog struct.")]
        Post getPost(
          string postid,
          string username,
          string password);

        [XmlRpcMethod("metaWeblog.getRecentPosts",
          Description = "Retrieves a list of the most recent existing post " +
                        "using the metaWeblog API. Returns the metaWeblog struct collection.")]
        Post[] getRecentPosts(
          object blogid,
          string username,
          string password,
          int numberOfPosts);

        [XmlRpcMethod("metaWeblog.newPost",
          Description = "Makes a new post to a designated blog using the " +
                        "metaWeblog API. Returns postid as a string.")]
        string newPost(
          object blogid,
          string username,
          string password,
          Post post,
          bool publish);

        [XmlRpcMethod("metaWeblog.newMediaObject",
          Description = "Uploads an image, movie, song, or other media " +
                        "using the metaWeblog API. Returns the metaObject struct.")]
        MediaObjectUrl newMediaObject(string blogid, string username, string password, MediaObject mediaobject);

        #region BloggerAPI Members


        [XmlRpcMethod("blogger.deletePost",
             Description = "Deletes a post.")]
        [return: XmlRpcReturnValue(Description = "Always returns true.")]
        bool deletePost(
            string appKey,
            string postid,
            string username,
            string password,
            [XmlRpcParameter(
                 Description = "Where applicable, this specifies whether the blog "
                 + "should be republished after the post has been deleted.")]
		  bool publish);

        [XmlRpcMethod("blogger.getUsersBlogs",
             Description = "Returns information on all the blogs a given user "
             + "is a member.")]
        BlogInfo[] getUsersBlogs(
            string appKey,
            string username,
            string password);

        #endregion
    }
}