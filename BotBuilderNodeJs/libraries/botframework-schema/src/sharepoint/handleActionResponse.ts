// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { CardViewResponse } from './cardViewResponse';
import { QuickViewResponse } from './quickViewResponse';


export type ViewResponseType = 'Card' | 'QuickView' | 'NoOp';

export interface BaseHandleActionResponse {
    responseType: ViewResponseType;
    renderArguments?: CardViewResponse | QuickViewResponse;
}

export interface CardViewHandleActionResponse extends BaseHandleActionResponse {
    responseType: 'Card';
    renderArguments: CardViewResponse;
}

export interface QuickViewHandleActionResponse extends BaseHandleActionResponse {
    responseType: 'QuickView';
    renderArguments: QuickViewResponse;
}

export interface NoOpHandleActionResponse extends BaseHandleActionResponse {
    responseType: 'NoOp';
    renderArguments?: undefined;
}

export type HandleActionResponse = CardViewHandleActionResponse | QuickViewHandleActionResponse | NoOpHandleActionResponse;