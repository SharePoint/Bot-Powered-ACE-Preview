// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IQuickViewData } from './IQuickViewData';
import { AdaptiveCard } from 'adaptivecards';

/**
 * Sharepoint GetQuickView response object
 */
export class GetQuickViewResponse {
    private data: IQuickViewData;
    private template: AdaptiveCard;
    private viewId: string;
    private stackSize: number;
    /**
     * Initializes a new instance of the GetQuickViewResponse class
     */
    public GetQuickViewResponse(){
        // Do nothing
    }

    /**
     * Sets data for the quick view of type IQuickViewData
     */
    public set Data(data: IQuickViewData){
        this.data = data;
    }

    /**
     * Gets data for the quick view of type IQuickViewData
     */
    public get Data(): IQuickViewData {
        return this.data;
    }

    /**
     * Sets the quick view template of type QuickViewTemplate
     */
    public set Template(template: AdaptiveCard){
        this.template = template;
    }

    /**
     * Gets the quick view template of type QuickViewTemplate
     */
    public get Template(): AdaptiveCard {
        return this.template;
    }

    /**
     * Sets view id property of type string
     */
    public set ViewId(viewId: string){
        this.viewId = viewId;
    }

    /**
     * Gets view id property of type string
     */
    public get ViewId(): string {
        return this.viewId;
    }

    /**
     * Sets stackSize property of type number
     */
    public set StackSize(stackSize: number){
        this.stackSize = stackSize;
    }

    /**
     * Gets stackSize property of type number
     */
    public get StackSize(): number {
        return this.stackSize;
    }
}