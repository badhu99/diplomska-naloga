export interface ISensorDetails {
    name:string,
    content:IContent[],
    xAxis:string,
    yAxis:string,
}

export interface IContent{
    name:string,
    series:any[]
}
