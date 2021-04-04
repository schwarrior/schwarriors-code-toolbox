const glob = require('glob');

const destinationFolder = 'lib';
const skipFiles = ['util','lib','.vscode','readme.md','license'];

console.log('Outputing git commands ');
console.log('for moving code files from project root to lib ...');
console.log('');

const handleFile = function (file) {
    const fileParts = file.split('/');
    const fileStart = fileParts[0].toLowerCase();
    if (skipFiles.indexOf(fileStart) > -1) { return }
    const mvFile = `${destinationFolder}\\${file}`;
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
        console.log('');
        console.log('Done.');
    }
}

const getDirectories = function (onComplete) {
    glob('*', onComplete);
  };

getDirectories(globComplete);
