const glob = require('glob');

const destinationFolder = 'lib';
const skipFiles = ['util','.vscode','package.json','package-lock.json','README.md','license'];

console.log('Moving files from project root to library ...');
console.log();

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
        console.log('Done.');
    }
}

const getDirectories = function (onComplete) {
    glob('*', { cwd: "../../" }, onComplete);
  };

getDirectories(globComplete);
