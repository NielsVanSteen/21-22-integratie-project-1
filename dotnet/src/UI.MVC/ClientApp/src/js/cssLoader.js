// Custom CSS imports
// import '../css/site.css';
import '../css/site.scss';

import '../css/pages/login.scss';
import '../css/pages/createModerator.scss';
import '../css/pages/notFound404.scss';
import '../css/pages/registration.scss';
import '../css/pages/profile.scss';
import '../css/pages/projectModeration.scss';
import '../css/pages/projectTags.scss';
import '../css/pages/registrationInformationEdit.scss';
import '../css/pages/moderators.scss';
import '../css/pages/docReview.scss';
import '../css/pages/projectActivity.scss';
import '../css/pages/projectManage.scss';
import '../css/pages/statistics.scss';
import '../css/pages/projectSettings.scss';
import '../css/pages/analyseComments.scss';
import '../css/pages/timeline.scss';
import '../css/pages/userProjectPage.scss';
import '../css/pages/projectStyling.scss';
import '../css/pages/userDocReviewPage.scss';

// Apply the current project style before the DOM is completely loaded.
import * as styleApplier from "./shared/styleApplier.js";
await styleApplier.checkReloadStyles();
styleApplier.applyStyles();


