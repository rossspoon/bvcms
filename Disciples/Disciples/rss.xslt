<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <xsl:apply-templates select="rss" />
    </html>
  </xsl:template>
  <xsl:template match="rss">
    <head>
      <title>
        <xsl:value-of select="/rss/channel/title" />
      </title>
      <style type="text/css">
        body { font-family: Verdana, Helvetica, sans-serif; font-size: 0.8em; }
        #headerInfo { text-align: center; width:90%; }
        #feedItems { width: 80%; border: 1px solid #333; padding-left: 15px; padding-right: 15px; padding-top: 5px; padding-bottom: 5px; text-align: left; }
        #headerText { text-align: center; padding: 8px; border: 1px dashed #bbb; background-color: #f7f7f7; }
        .rssDescription { padding-left: 25px; }
      </style>
    </head>
    <body>
      <div align="center">
        <div id="headerInfo">
          <h1>
            <xsl:value-of select="/rss/channel/title" />
          </h1>
          <p align="center" id="headerText">
            This is an RSS syndication feed for "<b><xsl:value-of select="/rss/channel/title" /></b>"
            which contains the most recent entries of this blog.
            It is intended to be viewed in a News Aggregator.
            You can subscribe to this feed by copying the address of this page to
            your favorite news aggregator and be kept abreast of the latest articles from this site.
          </p>
        </div>
      </div>
      <br />
      <div align="center">
        <div id="feedItems">
          <xsl:apply-templates select="channel/item" />
        </div>
      </div>
    </body>
  </xsl:template>
  <xsl:template match="channel/item">
    <div class="rssItem">
      <h2 class="rssTitle">
        <a href="{link}">
          <xsl:value-of select="title" />
        </a>
      </h2>
      <div name="decodeable" class="rssDescription">
        <xsl:value-of select="description" disable-output-escaping="yes" />
      </div>
    </div>
  </xsl:template>
</xsl:stylesheet>