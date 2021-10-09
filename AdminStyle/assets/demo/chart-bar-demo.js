// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';



//BAR : Most sales in country
$(document).ready(
    function () {
        var url = "/Dashboard/GetSalesPerDistrict";
        $.get(url, function (res) {
            new Morris.Bar({
                element: 'myBarChart',
                data: res,
                xkey: 'District',
                ykeys: ['Sales_Amount'],
                labels: ['Sales Amount'],
                barRatio: 0.4,
                xLabelAngle: 35,
                hideHover: 'auto',
                resize: true
            })
        })
    }
);

