import http from 'k6/http';
import { check } from 'k6';

export const options = {
    vus: __ENV.VU_COUNT,
    duration: '1h'
};

const cities = [
    'newyork',
    'london',
    'dubai',
    'hongkong',
    'bend',
    'stlouis'
];

const includeRadarValues = [true, true, false];
const includeSatelliteValues = [true, true, true, true, true, false];

const users = [
    {
        name: 'Alice',
        city: 'london',
        includeRadar: true
    },
    {
        name: 'Bob',
        includeRadar: true,
        includeSatellite: true
    },
    {
        name: 'Charlie',
        city: 'dubai'
    },
    {
        name: 'David',
        includeRadar: false,
        includeSatellite: false
    },
    {
        name: 'Eve'
    }    
];

function getForecast(user, city, includeRadar, includeSatellite) {
    const response = http.get(
        `${__ENV.PROTOCOL}://${__ENV.WEATHER_SERVICE_HOSTNAME}/WeatherForecast?city=${city}&includeRadar=${includeRadar}&includeSatellite=${includeSatellite}`,
        {
            headers: {
                'weather-user': user.name
            }
        });
    check(response, { 'status was 200': (r) => r.status == 200 });
}

function getUser() {
    const r = Math.floor(Math.random() * 100);
    return users[r];
}

function getAttribute(user, attribute, backupValues) {
    if (Object.keys(user).indexOf(attribute) !== -1) {
        return user[attribute];
    } else {
        const r = Math.floor(Math.random() * backupValues.length);
        return backupValues[r];
    }
}

export default function () {
    const user = getUser();
    const city = getAttribute(user, 'city', cities);
    const includeRadar = getAttribute(user, 'includeRadar', includeRadarValues);
    const includeSatellite = getAttribute(user, 'includeSatellite', includeSatelliteValues);
    getForecast(user, city, includeRadar, includeSatellite);
}
