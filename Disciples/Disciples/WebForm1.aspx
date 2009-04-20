<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN"
        "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head profile="http://gmpg.org/xfn/1">
    <title>Eric's Archived Thoughts: Wanted: Excerpt Exacter</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <meta name="generator" content="WordPress 2.5.1">
    <!-- please leave this for stats -->
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link rel="shortcut icon" href="/favicon.ico">
    <link rel="home" href="http://meyerweb.com/" title="Home">
    <link rel="stylesheet" href="http://meyerweb.com/ui/meyerweb.css" type="text/css"
        media="screen, projection">
    <link rel="stylesheet" href="http://meyerweb.com/ui/theme.css" type="text/css" media="screen, projection"
        id="themeLink">
    <link rel="stylesheet" href="http://meyerweb.com/ui/print.css" type="text/css" media="print">

    <script src="http://meyerweb.com/ui/addresses.js" type="text/javascript"></script>

    <link rel="stylesheet" href="/ui/wordpress.css" type="text/css">
    <link rel="stylesheet" href="/ui/tfe.css" type="text/css">
    <link rel="pingback" href="http://meyerweb.com/eric/thoughts/xmlrpc.php">
    <link rel="EditURI" type="application/rsd+xml" title="RSD" href="http://meyerweb.com/eric/thoughts/xmlrpc.php?rsd" />
    <link rel="wlwmanifest" type="application/wlwmanifest+xml" href="http://meyerweb.com/eric/thoughts/wp-includes/wlwmanifest.xml" />
    <meta name="generator" content="WordPress 2.5.1" />
</head>
<body id="www-meyerweb-com" class="arch">
    <div id="sitemast">
        <h1>
            <a href="/"><span>meyerweb</span>.com</a></h1>
    </div>
    <div id="search">
        <h4>
            Exploration</h4>
        <form method="get" action="http://www.google.com/custom">
        <input type="submit" name="sa" value="Search">
        <input type="text" name="q" size="20" maxlength="255" value="">
        <input type="hidden" name="sitesearch" value="meyerweb.com">
        </form>
        <small><a href="http://www.google.com/search">Powered by Google</a></small>
    </div>
    <div id="main">
        <div class="skipper">
            Skip to: <a href="#navhead">site navigation/presentation</a></div>
        <div class="skipper">
            Skip to: <a href="#thoughts">Thoughts From Eric</a></div>
        <div id="thoughts">
            <div class="entry">
                <h3>
                    <a href="http://meyerweb.com/eric/thoughts/2008/06/10/wanted-excerpt-exacter/" rel="bookmark"
                        title="Permanent Link: Wanted: Excerpt Exacter">Wanted: Excerpt Exacter</a></h3>
                <ul class="meta">
                    <li class="date">Tue 10 Jun 2008</li>
                    <li class="time">2223</li>
                    <li class="cat"><a href="http://meyerweb.com/eric/thoughts/category/tech/tools/"
                        title="View all posts in Tools" rel="category tag">Tools</a><br>
                        <a href="http://meyerweb.com/eric/thoughts/category/tech/wordpress/" title="View all posts in WordPress"
                            rel="category tag">WordPress</a></li>
                    <li class="cmt"><a href="http://meyerweb.com/eric/thoughts/2008/06/10/wanted-excerpt-exacter/#comments">
                        16 responses</a></li>
                    <li></li>
                    <li></li>
                </ul>
                <div class="text">
                    <p>
                        So after I once again published <a href="http://meyerweb.com/eric/thoughts/2008/06/10/caught-in-the-camera-eye/">
                            a post</a> without filling in the excerpt, thus forcing me to go back to fill
                        it in later, I <a href="http://twitter.com/meyerweb/statuses/831561584">tweeted</a>
                        in a fit of pique:
                    </p>
                    <blockquote cite="http://twitter.com/meyerweb/statuses/831561584">
                        <p>
                            I need a WordPress plugin that won&#8217;t let me publish a post until I&#8217;ve
                            filled in the excerpt field. Anyone got one?
                        </p>
                    </blockquote>
                    <p>
                        To which I got a whole lot of responses saying, in effect, &#8220;Oooo! Good idea!
                        I need that too! Let me know when you find one!&#8221; Some of them came from people
                        running fairly high-profile blogs. The need clearly exists. A couple of responses
                        were of the &#8220;I could do that!&#8221; variety, so I thought I&#8217;d post
                        here so as to describe how I think it ought to work from the user&#8217;s perspective,
                        and then we can hash things out in comments and someone can code it up and make
                        everyone happy.
                    </p>
                    <p>
                        So really what I want is, when I push the &#8220;Publish&#8221; button in WordPress,
                        the plugin checks to see if there&#8217;s an excerpt. If not, one of two things
                        happens:
                    </p>
                    <ol>
                        <li>
                            <p>
                                The plugin throws up a warning dialog telling me that I&#8217;m about to post without
                                an excerpt (<em>again</em>). If I say &#8220;Okay&#8221;, it goes ahead with publishing.
                                If I say &#8220;Cancel&#8221;, it returns me right back to where I was, which is
                                the &#8220;Write Post&#8221; page, with all the data intact and unaltered.</p>
                        </li>
                        <li>
                            <p>
                                The plugin returns me to the &#8220;Write Post&#8221; page with all data intact
                                and unaltered, and puts an error box at the top of the page telling me I forgot
                                to write an excerpt (<em>again</em>) and that it won&#8217;t let me publish until
                                I fix the problem.</p>
                        </li>
                    </ol>
                    <p>
                        One or the other. I think I like #1 a little better, but I&#8217;d be good either
                        way. I&#8217;m open to other approaches as well, but I don&#8217;t think the plugin
                        should rely on JavaScript, as that means leaving out people who don&#8217;t enable
                        JavaScript or post from JS-incapable devices.
                    </p>
                    <p>
                        I would do this myself, but I&#8217;m a little wary of the &#8220;return to the
                        page with all data intact and unaltered&#8221; bit, which I would imagine is pretty
                        easy to mess up. Thus I&#8217;m putting it up here as a semi-Lazyweb post so that
                        someone else, someone with more experience with WordPress and plugin authoring,
                        can do it right and quickly.
                    </p>
                    <p>
                        Okay, who&#8217;s on it?
                    </p>
                </div>
            </div>
            <!-- You can start editing here. -->
            <div id="comments">
                <h3>
                    16 Responses<a href="#postcomment" title="Leave a comment">&raquo;</a>
                </h3>
                <ul id="rss-tb">
                    <li id="tb"><a href="http://meyerweb.com/eric/thoughts/2008/06/10/wanted-excerpt-exacter/trackback/">
                        Trackback URL</a></li>
                    <li><a href='http://meyerweb.com/eric/thoughts/2008/06/10/wanted-excerpt-exacter/feed/'>
                        Comments
                        <abbr title="Really Simple Syndication">
                            RSS</abbr>
                        feed</a></li>
                </ul>
                <ol>
                    <li class="response  alt" id="comment-384147">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384147"><small>#</small>1</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Tue 10 Jun 2008</li>
                            <li class="ct">2227</li>
                        </ul>
                        <div class="text">
                            <h5>
                                Shelly wrote in to say...</h5>
                            <p>
                                Ooo&#8230;I think that could be done. #1 would definitely be easier than #2.
                            </p>
                            <p>
                                I do love a challenge though :)</p>
                        </div>
                    </li>
                    <li class="response " id="comment-384148">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384148"><small>#</small>2</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Tue 10 Jun 2008</li>
                            <li class="ct">2230</li>
                        </ul>
                        <div class="text">
                            <h5>
                                <a href='http://hami.sh' rel='external nofollow'>Hamish M</a> wrote in to say...</h5>
                            <p>
                                I&#8217;m on it. Nothing like a small plugin challenge to keep the mind energized.</p>
                            <p>
                                Will report back when I have a working prototype. Shouldn&#8217;t be too long. :)</p>
                        </div>
                    </li>
                    <li class="response  alt" id="comment-384150">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384150"><small>#</small>3</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Tue 10 Jun 2008</li>
                            <li class="ct">2237</li>
                        </ul>
                        <div class="text">
                            <h5>
                                <a href='http://www.collisionbend.com' rel='external nofollow'>Will Kessel</a> wrote
                                in to say...</h5>
                            <p>
                                Eric, I can hand this off to a guy at work &#8212; and willingly so because Jim
                                (our fearless leader) wants every post to have an excerpt before it gets posted.
                                I&#8217;d jump on it myself except I&#8217;m pretty jammed up right now.</p>
                            <p>
                                As I twitted back to you this afternoon, I&#8217;d have it check (on-click) to see
                                if you had included an excerpt. If not, the best way to return you to the editing
                                page would be to first save the post as a Draft and take you back to the post edit
                                page. Else, you could publish it anyway.</p>
                            <p>
                                You could build into the admin page an option for the &#8220;post anyway&#8221;
                                or &#8220;never post without&#8221; choice and therefore have it both ways.</p>
                            <p>
                                It would be a simple test to look for the presence of the_excerpt() and run from
                                there.
                            </p>
                            <p>
                                Personally, I&#8217;d make it look cool by using a jQuery lightbox-style dialog
                                box with one of the pre-installed jQuery plugins in the WP Admin area, but that&#8217;s
                                just me playing with the eye candy&#8230; ;-)</p>
                        </div>
                    </li>
                    <li class="response pingback" id="comment-384156">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384156"><small>#</small>4</a></li>
                            <li class="ci">Pingback</li>
                            <li class="cd">Tue 10 Jun 2008</li>
                            <li class="ct">2323</li>
                        </ul>
                        <div class="text">
                            <h5>
                                Received from <a href='http://www.hoosgot.com/index.php/2008/06/10/wanted-excerpt-exacter/'
                                    rel='external nofollow'>Wanted: Excerpt Exacter</a></h5>
                            <p>
                                [...] <a href="http://meyerweb.com/eric/thoughts/2008/06/10/wanted-excerpt-exacter/"
                                    rel="nofollow">http://meyerweb.com/eric/thoughts/2008/06/10/wanted-excerpt-exacter/</a>
                                asks Hoosgot, [...]</p>
                        </div>
                    </li>
                    <li class="response  alt" id="comment-384160">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384160"><small>#</small>5</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Tue 10 Jun 2008</li>
                            <li class="ct">2330</li>
                        </ul>
                        <div class="text">
                            <h5>
                                Erik wrote in to say...</h5>
                            <p>
                                WP 2.5 admin uses jquery right? I bet you could just insert a little script that
                                gives you a confirmation box if the excerpt field is empty.</p>
                        </div>
                    </li>
                    <li class="response " id="comment-384172">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384172"><small>#</small>6</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Wed 11 Jun 2008</li>
                            <li class="ct">0056</li>
                        </ul>
                        <div class="text">
                            <h5>
                                <a href='http://hami.sh' rel='external nofollow'>Hamish M</a> wrote in to say...</h5>
                            <p>
                                Okay. Version 1.0 is ready.<br />
                                <a href="http://hamstu.com/uploads/NeverForgetcerpt.zip" rel="nofollow">http://hamstu.com/uploads/NeverForgetcerpt.zip</a></p>
                            <p>
                                <b>Some notes</b><br />
                                So yeah, it&#8217;s called <em>Never Forgetcerpt</em>, haha, maybe a little lame
                                but hey, who cares. It works as far as I can tell, only tested in Wordpress 2.5,
                                and it uses jQuery, so with older WP installs probably won&#8217;t work so well,
                                i.e. at all. See the readme.txt for more info.</p>
                            <p>
                                *glances at footer* &#8212; Whoah, Eric, are you really using WP 1.5? Hmm, probably
                                isn&#8217;t going to work for you. You gotta upgrade man!</p>
                            <p>
                                Anyway, it&#8217;s GNU licensed, anyone feel free to take it and do what you like
                                with it. I hope it&#8217;s useful to some.</p>
                            <p>
                                Cheers,<br />
                                Hamish</p>
                        </div>
                    </li>
                    <li class="response  alt" id="comment-384176">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384176"><small>#</small>7</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Wed 11 Jun 2008</li>
                            <li class="ct">0114</li>
                        </ul>
                        <div class="text">
                            <h5>
                                <a href='http://magazeta.com' rel='external nofollow'>Alexander Maltsev</a> wrote
                                in to say...</h5>
                            <p>
                                I use this &#8220;hack&#8221;, use it in loop:<br />
                                <code>&lt;?php if ( ($post-&gt;post_excerpt != '') &amp;&amp; (!is_single()) ) {the_excerpt();
                                    }<br />
                                    else the_content('Прочтём до конца?',strip_teaser);?&gt;</code></p>
                        </div>
                    </li>
                    <li class="response " id="comment-384213">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384213"><small>#</small>8</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Wed 11 Jun 2008</li>
                            <li class="ct">0333</li>
                        </ul>
                        <div class="text">
                            <h5>
                                <a href='http://www.travaganza.com.au/' rel='external nofollow'>Travis</a> wrote
                                in to say...</h5>
                            <p>
                                wow, the plug-in is already available even before I get to leave a comment&#8230;
                                Good job Hamish!</p>
                        </div>
                    </li>
                    <li class="response  alt" id="comment-384216">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384216"><small>#</small>9</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Wed 11 Jun 2008</li>
                            <li class="ct">0343</li>
                        </ul>
                        <div class="text">
                            <h5>
                                <a href='http://alastairc.ac/' rel='external nofollow'>AlastairC</a> wrote in to
                                say...</h5>
                            <p>
                                I have a related problem: The automated excerpt would be fine, <em>if</em> it included
                                pictures, and included the first paragraph rather than first X characters.</p>
                            <p>
                                90% of the time the above approach would be what I intended, the other 10% would
                                still be closer, but I would edit it slightly.</p>
                        </div>
                    </li>
                    <li class="response " id="comment-384217">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384217"><small>#</small>10</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Wed 11 Jun 2008</li>
                            <li class="ct">0344</li>
                        </ul>
                        <div class="text">
                            <h5>
                                <a href='http://alastairc.ac/' rel='external nofollow'>AlastairC</a> wrote in to
                                say...</h5>
                            <p>
                                &#8220;Whoah, Eric, are you really using WP 1.5?&#8221;</p>
                            <p>
                                The source says it&#8217;s the latest, so I&#8217;d assume the footer is just content.</p>
                        </div>
                    </li>
                    <li class="response  alt kahuna" id="comment-384236">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384236"><small>#</small>11</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Wed 11 Jun 2008</li>
                            <li class="ct">0458</li>
                        </ul>
                        <div class="text">
                            <h5>
                                <a href='http://www.meyerweb.com/' rel='external nofollow'>Eric Meyer</a> wrote
                                in to say...</h5>
                            <p>
                                Very cool, <a href="http://meyerweb.com/eric/thoughts/2008/06/10/wanted-excerpt-exacter/?#comment-384172"
                                    rel="nofollow">Hamish</a>! Thank you! There&#8217;s one thing about it that
                                worries me, which is that it doesn&#8217;t save anything. So if I accidentally close
                                my browser window because I see a little pop-up I want to dismiss and my muscle
                                memory kicks out a command-W, I&#8217;ll lose whatever changes I made right before
                                hitting &#8220;Publish&#8221;. 95% of the time, that won&#8217;t happen, but the
                                other 5% will be really annoying. Is there any way to modify things so that a save-draft
                                operation is fired right before putting up the warning box?</p>
                            <p>
                                Oh, and I&#8217;m on the latest version; I just hadn&#8217;t updated my footer.
                                Thanks for reminding me to get it fixed.</p>
                        </div>
                    </li>
                    <li class="response " id="comment-384257">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384257"><small>#</small>12</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Wed 11 Jun 2008</li>
                            <li class="ct">0838</li>
                        </ul>
                        <div class="text">
                            <h5>
                                <a href='http://8stars.org/' rel='external nofollow'>Adam Rice</a> wrote in to say...</h5>
                            <p>
                                This seems to be solved already, but Ecto (not a plugin) also optionally gives warnings
                                if you try to post without an excerpt, category, and/or (maybe?) tags.</p>
                        </div>
                    </li>
                    <li class="response  alt" id="comment-384294">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384294"><small>#</small>13</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Wed 11 Jun 2008</li>
                            <li class="ct">1023</li>
                        </ul>
                        <div class="text">
                            <h5>
                                <a href='http://hami.sh' rel='external nofollow'>Hamish M</a> wrote in to say...</h5>
                            <p>
                                Hey, thanks a lot <a href="http://meyerweb.com/eric/thoughts/2008/06/10/wanted-excerpt-exacter/#comment-384236"
                                    rel="nofollow">Eric</a>! I&#8217;m glad you like it.
                            </p>
                            <p>
                                I&#8217;ve now updated the plugin to autosave when you click either Save or Publish.
                                Same address: <a href="http://hamstu.com/uploads/NeverForgetcerpt.zip" rel="nofollow">
                                    http://hamstu.com/uploads/NeverForgetcerpt.zip</a></p>
                            <p>
                                Also, I&#8217;m glad to hear you&#8217;re not actually using Wordpress 1.5. You
                                had me worried there for a minute!</p>
                        </div>
                    </li>
                    <li class="response  kahuna" id="comment-384300">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384300"><small>#</small>14</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Wed 11 Jun 2008</li>
                            <li class="ct">1034</li>
                        </ul>
                        <div class="text">
                            <h5>
                                <a href='http://www.meyerweb.com/' rel='external nofollow'>Eric Meyer</a> wrote
                                in to say...</h5>
                            <p>
                                So awesome, <a href="http://meyerweb.com/eric/thoughts/2008/06/10/wanted-excerpt-exacter/?#comment-384294"
                                    rel="nofollow">Hamish</a>. You rock.</p>
                            <p>
                                Now, what do you think of extending your excellent work into a more complex plugin
                                called (for example) &#8220;Forget-Meta-Not&#8221; which lets the user set preferences
                                to warn them if they&#8217;ve forgotten any of excerpt, categories, or tags? (Inspired
                                by both my further thinking and <a href="http://meyerweb.com/eric/thoughts/2008/06/10/wanted-excerpt-exacter/?#comment-384257"
                                    rel="nofollow">Adam</a> mentioning what Ecto does.)</p>
                        </div>
                    </li>
                    <li class="response  alt" id="comment-384305">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384305"><small>#</small>15</a></li>
                            <li class="ci">Comment</li>
                            <li class="cd">Wed 11 Jun 2008</li>
                            <li class="ct">1125</li>
                        </ul>
                        <div class="text">
                            <h5>
                                <a href='http://hami.sh' rel='external nofollow'>Hamish M</a> wrote in to say...</h5>
                            <p>
                                Great idea. I know I&#8217;ve forgotten categories, tags, etc. before as well, so
                                that would be quite useful.</p>
                            <p>
                                I&#8217;ll see what I can do.</p>
                        </div>
                    </li>
                    <li class="response pingback" id="comment-384486">
                        <ul class="meta">
                            <li class="cn"><a href="#comment-384486"><small>#</small>16</a></li>
                            <li class="ci">Pingback</li>
                            <li class="cd">Thu 12 Jun 2008</li>
                            <li class="ct">2052</li>
                        </ul>
                        <div class="text">
                            <h5>
                                Received from <a href='http://wplover.com/neverforgetcerpt-plugi/' rel='external nofollow'>
                                    NeverForgetcerpt Plugin :: WPLover</a></h5>
                            <p>
                                [...] begins from CSS godfather Eric Meyer&#8217;s tweet a few days ago, which then
                                turns into a blog post: NeverForgetcerpt, a plugin that will warn you if you&#8217;re
                                posting an article without creating [...]</p>
                        </div>
                    </li>
                </ol>
                <div id="postcomment">
                    <form action="http://meyerweb.com/eric/thoughts/2008/06/10/wanted-excerpt-exacter/#comment-395000"
                    method="post" id="commentform">
                    <h3>
                        Leave a Comment</h3>
                    <div id="fields">
                        <p>
                            <input type="text" name="author" id="author" class="textarea" value="" size="28"
                                tabindex="1">
                            <label for="author">
                                Name</label>
                            <input type="hidden" name="comment_post_ID" value="910">
                            <input type="hidden" name="redirect_to" value="/eric/thoughts/2008/06/10/wanted-excerpt-exacter/">
                        </p>
                        <p>
                            <input type="text" name="email" id="email" value="" size="28" tabindex="2">
                            <label for="email">
                                E-mail</label>
                        </p>
                        <p>
                            <input type="text" name="url" id="url" value="" size="28" tabindex="3">
                            <label for="url">
                                <acronym title="Uniform Resource Identifier">URI</acronym></label>
                        </p>
                    </div>
                    <p id="form-info">
                        Line and paragraph breaks automatic, e-mail address required but never displayed,
                        <acronym title="Hypertext Markup Language">HTML</acronym> allowed: <code>&lt;a href=&quot;&quot;
                            title=&quot;&quot;&gt; &lt;abbr title=&quot;&quot;&gt; &lt;acronym title=&quot;&quot;&gt;
                            &lt;b&gt; &lt;blockquote cite=&quot;&quot;&gt; &lt;cite&gt; &lt;code&gt; &lt;del
                            datetime=&quot;&quot;&gt; &lt;em&gt; &lt;i&gt; &lt;q cite=&quot;&quot;&gt; &lt;strike&gt;
                            &lt;strong&gt; </code>
                    </p>
                    <hr>
                    <!--<p><small><strong>XHTML:</strong> You can use these tags: &lt;a href=&quot;&quot; title=&quot;&quot;&gt; &lt;abbr title=&quot;&quot;&gt; &lt;acronym title=&quot;&quot;&gt; &lt;b&gt; &lt;blockquote cite=&quot;&quot;&gt; &lt;cite&gt; &lt;code&gt; &lt;del datetime=&quot;&quot;&gt; &lt;em&gt; &lt;i&gt; &lt;q cite=&quot;&quot;&gt; &lt;strike&gt; &lt;strong&gt; </small></p>-->
                    <p>
                        <label for="comment">
                            Your Comment</label>
                        <br>
                        <textarea name="comment" id="comment" cols="70" rows="10" tabindex="4"></textarea></p>
                    <p>
                        <strong>Remember to encode character entities if you're posting markup examples!</strong>
                        Management reserves the right to edit or remove any comment&mdash;especially those
                        that are abusive, irrelevant to the topic at hand, or made by anonymous posters&mdash;although
                        honestly, most edits are a matter of fixing mangled markup. Thus the note about
                        encoding your entities. If you're satisfied with what you've written, then go ahead...
                    </p>
                    <p>
                        <input id="preview" type="submit" name="preview" tabindex="5" value="Preview" /><input
                            id="submit" type="submit" name="submit" tabindex="6" style="font-weight: bold"
                            value="Post" />
                        <input type="hidden" name="comment_post_ID" value="910" />
                        <input type="hidden" name="mw_dssb" value="673b74b19071317cfa83c8713b523972" />
                    </p>
                    </form>
                </div>
                <ul class="prev-next" id="pn-bot">
                    <li class="prev">&larr; <a href="http://meyerweb.com/eric/thoughts/2008/06/10/caught-in-the-camera-eye/">
                        Caught In The Camera Eye</a></li>
                    <li class="main"><a href="/eric/thoughts/">Main</a></li>
                    <li class="next"><a href="http://meyerweb.com/eric/thoughts/2008/06/12/excerpts-exacted-shielding-the-admin/">
                        Excerpts Exacted; Shielding the Admin</a> &rarr;</li>
                </ul>
                <hr>
            </div>
        </div>
    </div>
    <div id="extra">
        <div id="calendar" class="panel">
            <table id="wp-calendar" cellspacing="0">
                <caption>
                    <a href="http://meyerweb.com/eric/thoughts/2008/06/" title="View all posts for this month">
                        June 2008</a></caption>
                <thead>
                    <tr>
                        <th abbr="$wd" scope="col" title="$wd" class="first">
                            S
                        </th>
                        <th abbr="$wd" scope="col" title="$wd">
                            M
                        </th>
                        <th abbr="$wd" scope="col" title="$wd">
                            T
                        </th>
                        <th abbr="$wd" scope="col" title="$wd">
                            W
                        </th>
                        <th abbr="$wd" scope="col" title="$wd">
                            T
                        </th>
                        <th abbr="$wd" scope="col" title="$wd">
                            F
                        </th>
                        <th abbr="$wd" scope="col" title="$wd" class="last">
                            S
                        </th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <td abbr="May" colspan="3" id="prevMonth" class="first">
                            <a href="http://meyerweb.com/eric/thoughts/2008/05/" title="View posts for May 2008">
                                May</a>
                        </td>
                        <td class="pad" id="thisMonth">
                            <a href="" title="Jump to latest posts"></a>
                        </td>
                        <td abbr="July" colspan="3" id="nextMonth" class="last">
                            <a href="http://meyerweb.com/eric/thoughts/2008/07/" title="View posts for July 2008">
                                July </a>
                        </td>
                    </tr>
                </tfoot>
                <tbody>
                    <tr>
                        <td class="first">
                            1
                        </td>
                        <td class="posts ">
                            <a href="http://meyerweb.com/eric/thoughts/2008/06/02/" title="The Missing Link">2</a>
                        </td>
                        <td>
                            3
                        </td>
                        <td>
                            4
                        </td>
                        <td class="posts ">
                            <a href="http://meyerweb.com/eric/thoughts/2008/06/05/" title="Strengthening Links">
                                5</a>
                        </td>
                        <td>
                            6
                        </td>
                        <td class="last">
                            7
                        </td>
                    </tr>
                    <tr>
                        <td class="first">
                            8
                        </td>
                        <td>
                            9
                        </td>
                        <td class="posts ">
                            <a href="http://meyerweb.com/eric/thoughts/2008/06/10/" title="Caught In The Camera Eye
Wanted: Excerpt Exacter">10</a>
                        </td>
                        <td>
                            11
                        </td>
                        <td class="posts ">
                            <a href="http://meyerweb.com/eric/thoughts/2008/06/12/" title="Excerpts Exacted; Shielding the Admin
Linking Up">12</a>
                        </td>
                        <td>
                            13
                        </td>
                        <td class="last">
                            14
                        </td>
                    </tr>
                    <tr>
                        <td class="first">
                            15
                        </td>
                        <td>
                            16
                        </td>
                        <td>
                            17
                        </td>
                        <td class="posts ">
                            <a href="http://meyerweb.com/eric/thoughts/2008/06/18/" title="Welcome">18</a>
                        </td>
                        <td>
                            19
                        </td>
                        <td>
                            20
                        </td>
                        <td class="last">
                            21
                        </td>
                    </tr>
                    <tr>
                        <td class="first">
                            22
                        </td>
                        <td>
                            23
                        </td>
                        <td>
                            24
                        </td>
                        <td>
                            25
                        </td>
                        <td>
                            26
                        </td>
                        <td>
                            27
                        </td>
                        <td class="last">
                            28
                        </td>
                    </tr>
                    <tr>
                        <td class="first">
                            29
                        </td>
                        <td>
                            30
                        </td>
                        <td class='pad' colspan='5'>
                            &nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="panel" id="post-nav">
            <h4>
                Sidestep</h4>
            <ul>
                <li class="next">Next entry:<br>
                    <a href="http://meyerweb.com/eric/thoughts/2008/06/12/excerpts-exacted-shielding-the-admin/">
                        Excerpts Exacted; Shielding the Admin</a></li>
                <li class="prev">Previous entry:<br>
                    <a href="http://meyerweb.com/eric/thoughts/2008/06/10/caught-in-the-camera-eye/">Caught
                        In The Camera Eye</a></li>
            </ul>
        </div>
        <div class="panel" id="tfe-search">
            <h4>
                Search Eric's Archived Thoughts</h4>
            <form id="searchform" method="get" action="/eric/thoughts/index.php">
            <div>
                <input type="text" name="s" size="15">
                <input type="submit" name="submit" value="search">
            </div>
            </form>
        </div>
        <div class="panel">
            <h4>
                Feeds</h4>
            <ul>
                <li>Posts:
                    <ul>
                        <li><a href="http://meyerweb.com/feed/" title="Syndicate this site using RSS"><acronym
                            title="Really Simple Syndication">RSS</acronym> 2.0</a></li>
                        <li><a href="http://meyerweb.com/feed/rss/" title="Syndicate this site using RSS"><acronym
                            title="Really Simple Syndication">RSS</acronym> 0.92</a></li>
                        <li><a href="http://meyerweb.com/feed/atom/" title="Syndicate this site using Atom">
                            Atom</a></li>
                    </ul>
                    <li>Comments:
                        <ul>
                            <li><a href="http://meyerweb.com/comments/feed/" title="The latest comments to all posts in RSS">
                                <acronym title="Really Simple Syndication">RSS</acronym> 2.0</a></li>
                        </ul>
                    </li>
            </ul>
        </div>
        <div class="panel" id="extras">
            <h4>
                Extras</h4>
            <ul>
                <li><a href="/feeds/">Feeds</a> &#8226;</li>
                <li><a href="/eric/faq.html">FAQ</a> &#8226;</li>
                <li><a href="/family.html">Family</a></li>
            </ul>
        </div>
    </div>
    <div id="navigate">
        <h4>
            Navigation</h4>
        <ul id="navlinks">
            <li id="archLink"><a href="/eric/thoughts/">Archives</a></li>
            <li id="cssLink"><a href="/eric/css/">CSS</a></li>
            <li id="toolsLink"><a href="/eric/tools/">Toolbox</a></li>
            <li id="writeLink"><a href="/eric/writing.html">Writing</a></li>
            <li id="speakLink"><a href="/eric/talks/">Speaking</a></li>
            <li id="otherLink"><a href="/other/">Leftovers</a></li>
            <li id="aboutsite"><a href="/ui/about.html">About this site</a></li>
        </ul>
    </div>
    <div id="footer">
        <p class="sosumi">
            All contents of this site, unless otherwise noted, are &copy;1995-2008 <strong>Eric
                A. and Kathryn S. Meyer</strong>. All Rights Reserved.</p>
        <p>
            "<a href="/eric/thoughts/">Thoughts From Eric</a>" is powered by the &uuml;bercool
            <a href="http://wordpress.org/">WordPress</a>.
        </p>
        <!-- 27 queries. 0.234 seconds. -->
    </div>
</body>
</html>
