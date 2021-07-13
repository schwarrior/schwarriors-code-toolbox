// for use in browser
// use from HTML example:
/*
<button type="button" class="btn btn-secondary btn-sm" 
onclick="innerTextToClipboardById('sample-config')">
Copy to clipboard</button>
<pre><code id="sample-config">
  &lt;cb:scope ProjectName=&quot;Sample_Project&quot;&gt;
    &lt;project name=&quot;Sample_Project&quot; queue=&quot;A&quot;&gt;
    &lt;/project&gt;
  &lt;/cb:scope&gt;</code></pre>
*/

async function innerTextToClipboardById(source) {
    try {
      const sourceElem = document.getElementById(source);
      const sourceText = sourceElem.innerText;
      await navigator.clipboard.writeText(sourceText);
      console.log(`${source} text copied to the clipboard`);
    } catch (ex) {
      console.log(`${source} could not be copied to the clipboard. ${ex}`);
    }
  }