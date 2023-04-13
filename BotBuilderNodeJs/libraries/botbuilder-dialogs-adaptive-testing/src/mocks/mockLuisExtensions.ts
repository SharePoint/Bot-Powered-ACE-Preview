/* eslint-disable security/detect-non-literal-fs-filename */
/**
 * @module botbuilder-dialogs-adaptive-testing
 */
/**
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

import * as fs from 'fs';
import * as path from 'path';

/**
 * Convert settings from luis.settings.*.json into flattened settings.
 *
 * @example
 * Original:
 * {
 *     "luis": {
 *         "test_en_us_lu": "00000000-0000-0000-0000-000000000000"
 *     }
 * }
 * Flattened:
 * {
 *     "luis:test_en_us_lu": "00000000-0000-0000-0000-000000000000"
 * }
 *
 * @param {Record<string, any>} settings Original settings.
 * @returns {Record<string, string>} Flattened settings.
 */
function flattenSettings(settings: Record<string, any>): Record<string, string> {
    const config = {};
    for (const [key, value] of Object.entries(settings)) {
        if (typeof value === 'object') {
            const flatObject = flattenSettings(value);
            for (const [childKey, childValue] of Object.entries(flatObject)) {
                config[key + ':' + childKey] = childValue;
            }
        } else {
            config[key] = value;
        }
    }
    return config;
}

/**
 * Setup configuration to utilize the settings file generated by lubuild.
 *
 * @param {string} directory Directory with settings file in it.
 * @param {string} endpoint Endpoint to use with a default of westus.
 * @returns {Record<string, string>} Modified configuration.
 */
export function useMockLuisSettings(
    directory: string,
    endpoint = 'https://westus.api.cognitive.microsoft.com'
): Record<string, string> {
    const files = fs.readdirSync(directory);
    const settings = files
        .filter((file) => /^luis\.settings\..*\.json$/.test(file))
        .reduce((config, filename) => {
            const content = fs.readFileSync(path.join(directory, filename), 'utf-8');
            Object.assign(config, flattenSettings(JSON.parse(content)));
            return config;
        }, {});
    settings['luis:endpoint'] = endpoint;
    settings['luis:resources'] = directory;
    // Ensure there is a key even if there is no secret
    settings['luis:endpointKey'] = '00000000-0000-0000-0000-000000000000';
    return settings;
}
