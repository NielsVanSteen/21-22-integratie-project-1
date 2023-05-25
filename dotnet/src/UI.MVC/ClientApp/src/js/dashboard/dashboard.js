import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";

window.addEventListener("load", init)

import {
    Chart,
    ArcElement,
    LineElement,
    BarElement,
    PointElement,
    BarController,
    BubbleController,
    DoughnutController,
    LineController,
    PieController,
    PolarAreaController,
    RadarController,
    ScatterController,
    CategoryScale,
    LinearScale,
    LogarithmicScale,
    RadialLinearScale,
    TimeScale,
    TimeSeriesScale,
    Decimation,
    Filler,
    Legend,
    Title,
    Tooltip,
    SubTitle
} from 'chart.js';

Chart.register(
    ArcElement,
    LineElement,
    BarElement,
    PointElement,
    BarController,
    BubbleController,
    DoughnutController,
    LineController,
    PieController,
    PolarAreaController,
    RadarController,
    ScatterController,
    CategoryScale,
    LinearScale,
    LogarithmicScale,
    RadialLinearScale,
    TimeScale,
    TimeSeriesScale,
    Decimation,
    Filler,
    Legend,
    Title,
    Tooltip,
    SubTitle
);

// List of all charts -> before redrawing all the charts they must be destroyed first!
let charts = [];


/**
 * function executed on page load.
 * @author Niels Van Steen
 * */
function init() {
    getStatisticsOverview();

    // Event listener to reload the statistics.
    const btnReload = document.getElementById("btnReloadStatistics");
    btnReload.addEventListener("click", () => {
        reload();
    });
} // init.

function reload() {

    // Destroy all charts. before loading the new ones.
    charts.forEach(chart => {
        chart.destroy();
    });
    charts = [];

    // Remove all the added sections.
    const sections = document.querySelectorAll(".project-statistics-loaded-item");
    sections.forEach(section => {
        section.remove();
    });

    // Reload the statistics.
    getStatisticsOverview();
}

/**
 * Get all the project statistics overview via an http get-request.
 * @author Niels Van Steen
 * */
function getStatisticsOverview() {
    
    let detail = document.querySelector("#statisticsDetail").value;
    
    if (detail == null || detail === "" || detail === undefined || isNaN(parseFloat(detail))) {
        detail = 2;
    }
    
    // Create the object.
    const filter = {
        detail: detail,
        beginDate: document.querySelector("#beginDate").value,
        useBeginDate: document.querySelector("#checkBeginDate").checked,
        endDate: document.querySelector("#endDate").value,
        useEndDate: document.querySelector("#checkEndDate").checked,
    };
    const queryString = url.objectToQueryString(filter);

    fetch(url.url() + url.getProjectName() + "/ProjectStatistics/Overview?" + queryString, {
        method: "GET",
        headers: {
            "Accept": "application/json"
        }
    })
        .then(response => response.json())
        .then(data => showStatisticsOverview(data))
    
} // getStatisticsOverview.

/**
 * Show all the project-statistics overview.
 * @author Niels Van Steen
 * */
function showStatisticsOverview(stats) {

    const labels = stats.map(s => s.lastUpdated);

    showCommentStatusTypeOverview(stats, labels);
    showEmojiTypeOverview(stats, labels);
    showDocReviewStatusTypeOverview(stats, labels);

    // The first overview element.
    drawMiniChart("#totalCommentsChart", labels, stats.map(s => s.reactionGroupAmount));
    let first = stats[0].reactionGroupAmount;
    let last = stats[stats.length - 1].reactionGroupAmount;
    showOverviewText("#totalCommentsOverview", first, last, stats[stats.length - 1].reactionGroupAmountFormatted);

    // The second overview element.
    drawMiniChart("#totalEmojiChart", labels, stats.map(s => s.emojiAmount));
    first = stats[0].emojiAmount;
    last = stats[stats.length - 1].emojiAmount;
    showOverviewText("#totalEmojiOverview", first, last, stats[stats.length - 1].emojiAmountFormatted);

    // The third overview element.
    drawMiniChart("#totalUsersChart", labels, stats.map(s => s.usersAmount));
    first = stats[0].usersAmount;
    last = stats[stats.length - 1].usersAmount;
    showOverviewText("#totalUsersOverview", first, last, stats[stats.length - 1].usersAmountFormatted);

    // The 4th overview element.
    drawMiniChart("#totalManagersChart", labels, stats.map(s => s.managersAmount));
    first = stats[0].managersAmount;
    last = stats[stats.length - 1].managersAmount;
    showOverviewText("#totalManagersOverview", first, last, stats[stats.length - 1].managersAmountFormatted);

    // The 5th overview element.
    drawMiniChart("#totalDocReviewsChart", labels, stats.map(s => s.docReviewsAmount));
    first = stats[0].docReviewsAmount;
    last = stats[stats.length - 1].docReviewsAmount;
    showOverviewText("#totalDocReviewsOverview", first, last, stats[stats.length - 1].docReviewsAmountFormatted);

    for (let i = 0; i < types.length; i++) {
        drawMiniChart(`#total${types[i]}Chart`, labels, maps[i]);
    }

} // showStatisticsOverview.

let types = [];
let maps = [];

function showEmojiTypeOverview(stats, labels) {

    const rootElement = document.querySelector(".project-statistics-emoji-container");

    for (let i = 0; i < stats[0].emojiTypeAmount.length; i++) {
        let type = [];
        for (let e = 0; e < stats.length; e++) {
            if (stats[e] !== undefined)
                type.push(stats[e].emojiTypeAmount[i])
        }
        type = type.filter(element => {
            return element !== undefined;
        });

        const statusText = type[0].emojiCode;

        rootElement.innerHTML += `
            <section class="project-statistics-item project-statistics-loaded-item" id="total${statusText}Overview" data-statistic-type="2" data-active-statistic="${statusText}">
                <div class="statistics-numbers-container">
                    <h3></h3>
                    <p></p>
                </div>
                <div class="statistics-name">
                    <p>Emoji: &#${statusText};</p>
                </div>
                <div class="small-graph">
                    <canvas id="total${statusText}Chart"></canvas>
                </div>
            </section>`;

        const first = type[0].total;
        const last = type[type.length - 1].total;
        showOverviewText(`#total${statusText}Overview`, first, last, type[type.length - 1].totalFormatted);
        types.push(statusText);
        maps.push(type.map(s => s.total));
    } // For i.

} // showEmojiTypeOverview.

/**
 * Show the charts for all the different comment status types.
 * */
function showCommentStatusTypeOverview(stats, labels) {

    const rootElement = document.querySelector(".project-statistics-comment-container");

    for (let i = 0; i < 6; i++) {
        let type = [];
        for (let e = 0; e < stats.length; e++) {
            if (stats[e] !== undefined)
                type.push(stats[e].commentStatusTypeAmount[i])
        }
        type = type.filter(element => {
            return element !== undefined;
        });

        if (type.length === 0)
            continue;

        const statusText = type[0].commentStatusString;
        const statusText2 = statusText + "_Comments";

        rootElement.innerHTML += `
            <section class="project-statistics-item project-statistics-loaded-item" id="total${statusText2}Overview" data-statistic-type="1" data-active-statistic="${statusText}">
                <div class="statistics-numbers-container">
                    <h3></h3>
                    <p></p>
                </div>
                <div class="statistics-name">
                    <p>${statusText} Comments</p>
                </div>
                <div class="small-graph">
                    <canvas id="total${statusText2}Chart"></canvas>
                </div>
            </section>`;

        const first = type[0].total;
        const last = type[type.length - 1].total;
        showOverviewText(`#total${statusText2}Overview`, first, last, type[type.length - 1].totalFormatted);
        types.push(statusText2);
        maps.push(type.map(s => s.total));
    } // For i.
} // showCommentStatusTypeOverview.

/**
 * Show the charts for all the different comment status types.
 * */
function showDocReviewStatusTypeOverview(stats, labels) {


    const rootElement = document.querySelector(".project-statistics-doc-review-container");

    for (let i = 0; i < 4; i++) {
        let type = [];
        for (let e = 0; e < stats.length; e++) {
            if (stats[e] !== undefined)
                type.push(stats[e].docReviewStatusTypeAmount[i])
        }
        type = type.filter(element => {
            return element !== undefined;
        });

        if (type.length === 0)
            continue;

        const statusText = type[0].docReviewStatusString;

        rootElement.innerHTML += `
            <section class="project-statistics-item project-statistics-loaded-item" id="total${statusText}Overview" data-statistic-type="3" data-active-statistic="${statusText}">
                <div class="statistics-numbers-container">
                    <h3></h3>
                    <p></p>
                </div>
                <div class="statistics-name">
                    <p>${statusText} Doc-reviews</p>
                </div>
                <div class="small-graph">
                    <canvas id="total${statusText}Chart"></canvas>
                </div>
            </section>`;

        const first = type[0].total;
        const last = type[type.length - 1].total;
        showOverviewText(`#total${statusText}Overview`, first, last, type[type.length - 1].totalFormatted);
        types.push(statusText);
        maps.push(type.map(s => s.total));
    } // For i.
} // showCommentStatusTypeOverview.

/**
 * Show all the project-statistics overview.
 * @author Niels Van Steen
 * */
function showOverviewText(overviewClass, first, last, lastFormatted) {
    let overviewElement = document.querySelector(overviewClass);
    const difference = last - first;
    let increase = difference / first * 100;
    const sign = increase > 0 ? "+" : "";

    if (increase === Infinity)
        increase = 100;
    
    if (isNaN(increase))
        increase = 0;
    if (isNaN(lastFormatted))
        lastFormatted = 0;
    
    overviewElement.querySelector("h3").innerHTML = lastFormatted;
    overviewElement.querySelector(".statistics-numbers-container p").innerHTML = `(${sign}${Math.round(increase)}%)`;
} // showOverviewText.

/**
 * The chart.js chart with all the options. for the mini charts.
 * @author Niels Van Steen
 * */

function drawMiniChart(cls, labels, data) {

    try {
        let ctx = document.querySelector(cls).getContext("2d");
        ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);

        let chartType = document.querySelector("#selectChartType").value;
        if (chartType === "line") {
            chartType = "line";
        } else {
            chartType = "bar";
        }


        const chart = new Chart(ctx, {
            type: chartType,
            data: {
                labels: labels,
                datasets: [{
                    data: data,
                    fill: false,
                    borderColor: "#ffffff",
                    pointBackgroundColor: "transparent",
                    pointBorderColor: "#ffffff",
                    pointHoverBackgroundColor: "#000000",
                    pointHoverBorderColor: "#000000",
                }]
            },
            options: {
                responsive: true,
                pointRadius: 5,
                pointHitRadius: 5,
                elements: {
                    line: {
                        tension: 0.3
                    }
                },
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    x: {
                        display: false,
                        grid: {
                            display: false
                        }
                    },
                    y: {
                        display: false,
                        grid: {
                            display: false
                        },
                        ticks: {
                            precision: 0
                        }
                    }
                },
            }
        });
        charts.push(chart);

    } catch (e) {
    }

} // drawMiniChart.