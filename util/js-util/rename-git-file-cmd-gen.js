const glob = require('glob');

console.log('Outputing git commands ');
console.log('for renaming files');
console.log();

const handleFile = function (file) {
    const findThis = 'Csharp Tracing Logging Trace Switching'
    const replaceWith = 'Csharp Trace'
    const mvFile = file.replace(findThis, replaceWith);
    console.log(`git mv "lib/dotNet Trace Logging/${file}" "${mvFile}"`);
}

const globComplete = function (err, files) {
    if (err) {
        console.log('Error', err);
    } else {
        console.log();
        files.forEach(file => {
            handleFile(file);
        }); 
        console.log('Done.');
    }
}

const getDirectories = function (onComplete) {
    glob('*.*', { cwd: "lib/dotNet Trace Logging" }, onComplete);
  };

getDirectories(globComplete);
