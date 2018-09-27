var graphWidthScale=5;
var graphHeightScale=2;
var graphMargin=20;
var graphBottomAxis=50;
function liorGraphCreate(containerId) {
d3.select("#"+containerId).attr("viewBox","0 0 "+(graphWidthScale*100+graphMargin*2)+" "+(graphHeightScale*100+graphMargin+graphBottomAxis));
var g1 = d3.select("#"+containerId).append("svg").attr("width",graphWidthScale*100+graphMargin*2).attr("height",graphHeightScale*100+graphMargin+graphBottomAxis);
g1.append("line").attr("y1",graphMargin+graphHeightScale*100+5).attr("y2",graphMargin+graphHeightScale*100+5).attr("x1",graphMargin).attr("x2",graphMargin+graphWidthScale*100).attr("stroke","#000000").attr("stroke-width",1);
g1.append("polyline").attr("class","graphfill").attr("fill","#d4f1ff").attr("stroke-width",0);
g1.append("polyline").attr("class","graphline").attr("fill","none").attr("stroke","#007cb7").attr("stroke-width",10);
}
function liorGraphUpdate(dat,dataxis,containerId) {
var srcMin = Math.min.apply(Math,dat);
var srcMax = Math.max.apply(Math,dat);
var srcRange = srcMax-srcMin;
var dstMin = 0;
var dstMax = 100;
if(srcMin > 0) {
	dstMin = 10;
}
var dstRange = dstMax-dstMin;
var rangeScale = srcRange/dstRange;
var datnew = new Array(dat.length);
for(var i=0;i<dat.length;i++) {
	datnew[i] = ((dat[i]-srcMin)/rangeScale)+dstMin;
}
var graphPoints = "";
for(var i=0;i<datnew.length;i++) {
	graphPoints+=(graphMargin+graphWidthScale*i*100/(datnew.length-1))+","+(graphMargin+graphHeightScale*(100-datnew[i]))+" ";
}
var g1 = d3.select("#"+containerId).select("svg");
g1.selectAll("text.l").data(dataxis).enter().append("text").text((d,i)=>{
	return (d);
}).attr("y",graphHeightScale*100+graphBottomAxis-10).attr("x",(d,i)=>{
	return (graphMargin+graphWidthScale*i*100/(dataxis.length-1))-10;
}).attr("r",8).attr("class","l").attr("fill","#000000").attr("font-family","arial").attr("font-size","8px").exit().remove();
g1.select(".graphfill").attr("points",(graphMargin+","+(graphMargin+100*graphHeightScale)+" "+graphPoints+(graphMargin+100*graphWidthScale)+","+(graphMargin+100*graphHeightScale)));
g1.select(".graphline").attr("points",graphPoints);
g1.selectAll("circle").data(datnew).enter().append("circle").attr("cy",(d,i)=>{
	return (graphMargin+graphHeightScale*(100-d));
}).attr("cx",(d,i)=>{
	return (graphMargin+graphWidthScale*i*100/(datnew.length-1));
}).attr("r",8).attr("fill","#ffffff").attr("stroke","#007cb7").attr("stroke-width",3).exit().remove();
g1.selectAll("text.p").data(datnew).enter().append("text").text((d,i)=>{
	return (dat[i]);
}).attr("y",(d,i)=>{
	return (graphMargin+graphHeightScale*(100-d))+5;
}).attr("x",(d,i)=>{
    var space = 17;
    if (i == datnew.length-1) space = -35;
	return (graphMargin+graphWidthScale*i*100/(datnew.length-1))+space;
}).attr("r",8).attr("class","p").attr("fill","#007cb7").attr("font-family","arial").attr("font-weight","bold").attr("font-size","14px").exit().remove();
}