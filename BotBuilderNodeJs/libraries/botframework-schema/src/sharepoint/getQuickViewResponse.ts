// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IQuickViewData } from './IQuickViewData';
import { AdaptiveCard } from 'adaptivecards';

/**
 * Sharepoint GetQuickView response object
 */
export class GetQuickViewResponse {
    /**
     * Initializes a new instance of the GetQuickViewResponse class
     */
    public GetQuickViewResponse(){
        // Do nothing
    }

    /**
     * Sets data for the quick view of type IQuickViewData
     */
    public set data(data: IQuickViewData){
        this.data = data;
    }

    /**
     * Gets data for the quick view of type IQuickViewData
     */
    public get data(): IQuickViewData {
        return this.data;
    }

    /**
     * Sets the quick view template of type QuickViewTemplate
     */
    public set template(template: AdaptiveCard){
        this.template = template;
    }

    /**
     * Gets the quick view template of type QuickViewTemplate
     */
    public get template(): AdaptiveCard {
        return this.template;
    }

    /**
     * Sets view id property of type string
     */
    public set viewId(viewId: string){
        this.viewId = viewId;
    }

    /**
     * Gets view id property of type string
     */
    public get viewId(): string {
        return this.viewId;
    }

    /**
     * Sets stackSize property of type number
     */
    public set stackSize(stackSize: number){
        this.stackSize = stackSize;
    }

    /**
     * Gets stackSize property of type number
     */
    public get stackSize(): number {
        return this.stackSize;
    }
}