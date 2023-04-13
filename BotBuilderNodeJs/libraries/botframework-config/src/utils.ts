/**
 * @module botframework-config
 *
 * Copyright(c) Microsoft Corporation.All rights reserved.
 * Licensed under the MIT License.
 */

/**
 * @private
 * @deprecated See https://aka.ms/bot-file-basics for more information.
 * @param value
 */
export function uuidValidate(value: string): boolean {
    return /^[0-9a-f]{8}-?[0-9a-f]{4}-?[1-5][0-9a-f]{3}-?[89ab][0-9a-f]{3}-?[0-9a-f]{12}$/.test(value);
}
