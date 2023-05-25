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

window.addEventListener("load", init);

import * as popup from "./../shared/popup.js";
import * as url from "./../shared/url.js";

/**
 * Function executed on page load.
 *
 * @author Niels Van Steen
 * */
async function init() {

    await loadSurveyStatistics();
}

/**
 * Show all the survey statistics.
 *
 * @author Niels Van Steen
 * */
async function loadSurveyStatistics() {

    // Get the statistics.
    const statistics = await getSurveyStatistics();

    // Show the statistic containers.
    showStatisticContainers(statistics);

    // Load the charts.
    loadCharts(statistics);

} // loadSurveyStatistics.

/**
 * Loads the survey statistics from the server.
 *
 * @author Niels Van Steen
 * */
async function getSurveyStatistics() {
    const response = await fetch(url.url() + url.getProjectName() + "/Surveys/GetStatistics/", {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    });

    if (response.ok)
        return await response.json();
    return undefined;
} // getSurveyStatistics


/**
 * Shows the statistic elements in the browser. (the chart is loaded later with chart.js).
 *
 * @author Niels Van Steen
 * */
function showStatisticContainers(statistics) {
    const container = document.querySelector(".survey-statistics-content-container");

    statistics.map(statistic => {
        container.innerHTML += `
            <section class="survey-statistics-item-container project-statistics-item">
                <div class="title-container">
                    <div class="title-inner">
                        <h3>${statistic.title}</h3>
                        <div class="description-container">
                            <p class="survey-description">${statistic.description}</p>
                        </div>
                    </div>
                    <p>${statistic.AreMultipleOptionsAllowed ? "Multiple Answer" : "Multiple Choice"}</p>
                </div>
                
                <button class="btn-icon btn-show-highlighted-text">
                    <i class="fa-solid fa-align-left"></i>
                    <div class="highlighted-text-container">
                        <p></p>
                        <p>Selected text: ${statistic.selectedText}</p>
                    </div>
                </button>
                
                <div class="statistics-chart-container">
                    <canvas id="surveyChart${statistic.surveyId}"></canvas>
                </div>
            </section>
        `;
    });
} // showStatisticContainers.

function loadCharts(statistics) {
    statistics.forEach(s => loadChart(s));
} // loadChart.

function loadChart(statistic) {
    let ctx = document.querySelector("#surveyChart" + statistic.surveyId).getContext("2d");
    const data = statistic.optionsStatistics.map(option => option.amount);
    const labels = statistic.optionsStatistics.map(option => option.option);
    

    const color = window.getComputedStyle(document.documentElement).getPropertyValue('--darkest-grey');

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                fill: true,
                borderColor: "#ffffff",
                backgroundColor: color,
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
                },
            },
            scales: {
                x: {
                    display: true,
                    grid: {
                        display: false
                    },
                    ticks: {
                        color: "black",
                    }
                },
                y: {
                    display: true,
                    grid: {
                        display: false
                    },
                    ticks: {
                        precision: 0,
                        color: "black",
                    }
                }
            },
        }
    });
}