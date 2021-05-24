// prerequisite package config cmds
// npm i typescript --save-dev
// npm i @types/node --save-dev
// npm i @types/dotenv --save-dev
// npm i mssql --save
// npm i @types/mssql --save-dev

// prerequisite .env file exists in project root with keys
// see constructor for required keys

import * as env from 'dotenv'
import * as sql from 'mssql'

interface ColumnResult {
    Schema: string,
    Table: string,
    Column: string,
    Type: string,
    Length: number 
}

export class SqlServerColumnInfo {
    
    private readonly dbConfig: sql.config    
    private readonly dbSchema: string
    private readonly getColumnQuery = 'SELECT TABLE_SCHEMA as "Schema", TABLE_NAME as "Table", '
        + 'COLUMN_NAME as "Column", DATA_TYPE as "Type", CHARACTER_MAXIMUM_LENGTH as "Length" '
        + 'FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = @dbSchema '  
        + "AND COLUMN_NAME NOT LIKE '!_%' ESCAPE '!'"


    constructor() {
        env.config()
        this.dbConfig = {
            user: process.env['dbUser'],
            password: process.env['dbPassword'],
            server: process.env['dbServer'],
            database: process.env['dbDatabase'],
            options: {trustServerCertificate: true}
        }
        this.dbSchema = process.env['dbSchema']
    }

    getInfo = async () : Promise<sql.IProcedureResult<ColumnResult>> => {
        let ps: sql.PreparedStatement
        try {
            const conn = await new sql.ConnectionPool(this.dbConfig).connect()
            ps = new sql.PreparedStatement(conn)
            ps.input('dbSchema', sql.VarChar)
            await ps.prepare(this.getColumnQuery)
            const paramSet = {'dbSchema': this.dbSchema}
            const result: sql.IProcedureResult<ColumnResult> = await ps.execute(paramSet)
            return result
        } catch (err) {
            console.log(err)
            return null
        } finally {
            if (ps) {ps.unprepare()}
        }
    }


}
