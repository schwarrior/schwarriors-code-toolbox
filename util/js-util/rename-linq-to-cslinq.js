const glob = require('glob');

console.log('Outputing git commands ');
console.log('for renaming files that end with .linq');
console.log('To cs.linq');
console.log();

const handleFile = function (file) {
    const fileParts = file.split('.');
    let fileRoot = '';
    for(let partIdx = 0; partIdx < fileParts.length - 1; partIdx ++) {
        const filePart = fileParts[partIdx];
        if (partIdx > 0) {
            fileRoot += '.';
        }
        fileRoot += filePart;
    }
    let mvFile = `${fileRoot}.cs.linq`;
    if (fileRoot.substring(0,2) === 'VB') {
        mvFile = `${fileRoot}.vb.linq`;
    }
    console.log(`git mv "${file}" "${mvFile}"`);
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
    glob('*.linq', { cwd: "lib" }, onComplete);
  };

getDirectories(globComplete);
