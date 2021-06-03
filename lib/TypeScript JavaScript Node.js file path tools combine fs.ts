// npm i @types/node --save-dev
// import * as fs from 'fs/promises'

export class FileManager {

    static readonly directorySeparator = '\\'

    static readonly altDirectorySeparator = '/'

    static normalizeDirectorySeparators = (path: string) : string => {
        return path.replace(FileManager.altDirectorySeparator, FileManager.directorySeparator)
    }

    static combine = (path1: string, path2: string) : string => {
        let p1 = FileManager.normalizeDirectorySeparators(path1)
        let p2 = FileManager.normalizeDirectorySeparators(path2)
        if (!p1.endsWith(FileManager.directorySeparator)) {
            p1 =  p1 + FileManager.directorySeparator
        }
        while (p2.startsWith(FileManager.directorySeparator)) {
            p2 =  p2.substring(1)
        }
        return p1 + p2
    }


}