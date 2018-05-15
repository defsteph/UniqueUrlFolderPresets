# Clean and unique URLs for EPiServer, with support for using ImageResizing.NET presets in a cleaner way.
[![Build status](https://ci.appveyor.com/api/projects/status/nixydjck1gqq6odg?svg=true)](https://ci.appveyor.com/project/defsteph/uniqueurlfolderpresets)

## Usage
Use whatever mechanism you want to prepend resizing presets to your image urls, or use one of the provided UrlHelper extensions
like ```@Url.ResizeImage(Model.MyImageContentReference, "my-preset")``` or ```@Url.PrependResizingInstruction(Model.MyImageUrlString, "my-preset")```.

## What is installed?
1. An ImageResizing.NET plugin is installed in web.config
```
		<resizer>
			<plugins>
				<add name="UniqueUrlFolderPresetsPlugin" />
			</plugins>
		</resizer>
```
2. A BlobHandler that intercepts calls and ensures that urls are routed correctly for images.
3. An HttpModule that ensures that the urls use a hash for content, or preset urls.

## How it works
Whenever a change is made to a media item, a new hash is generated. This hash is then used to uniquely identify this version of the item. If the media 
item is changed, the old url will generate a permanent redirect to the new url. This is done to enable long term caching for media. By default this 
cache header is set to 365 days, using max-age. This can be changed by adding an appSetting with key "uufp:CacheMaxAgeTimeSpan", and a value of a timespan
format string.

What does this have to do with image resizing? Nothing. But the fun doesn't stop here. To top things off, this add-on adds the ability to generate 
prettier URLs, without all that querystring dirt. By default, the preset keyword is "optimized", but this can be changed by adding an appSetting with key 
"uufp:BaseSegment", and a string value of your choosing.

If a request is made for a preset that doesn't exist, it will result in a 404.

Given that the defaults are left as-is, if there's an ImageResizer preset with the name "test", and a media item with the url "/globalassets/my-media.png",
calling the url "/optimized/test/globalassets/my-media.png" will issue a redirect to "/optimized/test/\<hash\>/globalassets/my-media.png", where \<hash\> is
an 8 character long calculated hash for that media item, based on its last saved date. That URL will then give you the media item, with the preset applied, and cache headers
that will cache the item for a year.

## Tests
There are a few tests for the PresetParser, for testing the regular expression.

## Release history

### Release 1.0.0
Initial release.

### Release 1.0.1
Fixes bug with Regular expression for parsing presets.

### Release 1.1.0
Added new helper for doing the rewriting, that can be used outside of the UrlHelperExtension.

### Release 1.2.0
Added method on IContentHashHelper to remove a hash from a URL.

### Release 1.2.1
Added Enabled property on IUniqueUrlFolderPresetsConfiguration to only run resizing rewrites in teh default context (i.e. not in edit/preview)