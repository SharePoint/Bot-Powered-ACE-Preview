// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IQuickViewData } from './IQuickViewData';
import { AdaptiveCard } from 'adaptivecards';
import { ExternalLinkParameters } from './externalLinkParameters';
import { FocusParameters } from './focusParameters';

/**
 * Sharepoint GetQuickView response object
 */
export class GetQuickViewResponse {
    private data: IQuickViewData;
    private template: AdaptiveCard;
    private viewId: string;
    private title: string = '';
    private externalLink: ExternalLinkParameters;
    private focusParameters: FocusParameters;

    /**
     * Initializes a new instance of the GetQuickViewResponse class
     */
    public GetQuickViewResponse() {
        // Do nothing
    }

    /**
     * Sets data for the quick view of type IQuickViewData
     */
    public set Data(data: IQuickViewData) {
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
    public set Template(template: AdaptiveCard) {
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
    public set ViewId(viewId: string) {
        this.viewId = viewId;
    }

    /**
     * Gets view id property of type string
     */
    public get ViewId(): string {
        return this.viewId;
    }

    /**
     * Sets title property of type string
     */
    public set Title(title: string) {
        this.title = title;
    }

    /**
     * Gets title property of type string
     */
    public get Title(): string {
        return this.title;
    }

    /**
     * Sets externalLink property of type ExternalLinkParameters
     */
    public set ExternalLink(externalLink: ExternalLinkParameters) {
        this.externalLink = externalLink;
    }

    /**
     * Gets externalLink property of type ExternalLinkParameters
     */
    public get ExternalLink(): ExternalLinkParameters {
        return this.externalLink;
    }

    /**
     * Sets focus parameters property of type FocusParameters
     */
    public set FocusParameters(focusParameters: FocusParameters) {
        this.focusParameters = focusParameters;
    }

    /**
     * Gets title property of type FocusParameters
     */
    public get FocusParameters(): FocusParameters {
        return this.focusParameters;
    }
}